namespace KobzaReferee.Application.Helpers;

public static class StandingsCalculator
{
    public static List<OverallParticipantStatisticsDto> CalculateOverallStandings(List<TournamentStatistics> tournaments)
    {
        var overallParticipantsStatistics = new Dictionary<string, OverallParticipantStatisticsDto>();

        foreach (var tournament in tournaments)
        {
            if (tournament.Standings == null || !tournament.Standings.Any())
            {
                continue;
            }

            var winnerTournamentScore = tournament.Standings.Min(p => p.TournamentScore);

            foreach (var participant in tournament.Standings)
            {
                if (!overallParticipantsStatistics.ContainsKey(participant.UserId))
                {
                    overallParticipantsStatistics[participant.UserId] = new OverallParticipantStatisticsDto()
                    {
                        UserId = participant.UserId
                    };
                }

                overallParticipantsStatistics[participant.UserId].TotalPointsForStanding += participant.PointsForStanding;
                overallParticipantsStatistics[participant.UserId].TournamentCount++;

                if (participant.TournamentScore == winnerTournamentScore)
                {
                    overallParticipantsStatistics[participant.UserId].TotalWins++;
                }
            }
        }

        var result = overallParticipantsStatistics.Values.ToList();

        foreach (var participant in result)
        {
            participant.AveragePointsForStanding = participant.TotalPointsForStanding / (double)participant.TournamentCount;
        }

        return result;
    }

    public static List<TournamentParticipantStatistics> CalculateTournamentStandings(List<WordGuess> wordGuesses)
    {
        var participants = new List<TournamentParticipantStatistics>();

        var participantsWithDailyGuesses = wordGuesses.GroupBy(wg => wg.UserId);

        foreach (var participantWithDailyGuesses in participantsWithDailyGuesses)
        {
            var participant = new TournamentParticipantStatistics(participantWithDailyGuesses.Key);
            var participantTotalGuessTime = TimeSpan.Zero;

            foreach (var dailyGuess in participantWithDailyGuesses)
            {
                if (dailyGuess.Guesses.Any())
                {
                    int dailyPoints = CalculateDailyPoints(dailyGuess);

                    if (participant.ScoreByDate.TryAdd(dailyGuess.Date.Date, dailyPoints))
                    {
                        participant.TournamentScore += dailyPoints;

                        var guessTime = dailyGuess.SubmittedAt - dailyGuess.Date;
                        participantTotalGuessTime = participantTotalGuessTime.Add(guessTime.Duration());
                    }
                }
            }

            participant.AverageGuessTime = participantTotalGuessTime.Divide(participant.ScoreByDate.Count);

            participants.Add(participant);
        }

        participants = participants
            .OrderBy(p => p.TournamentScore)
            .ThenBy(p => p.AverageGuessTime)
            .ToList();

        int currentPoints = 1;
        int lastScore = -1;
        for (int i = 0; i < participants.Count; i++)
        {
            if (i > 0 && participants[i].TournamentScore != lastScore)
            {
                currentPoints++;
            }

            participants[i].StandingsPosition = i + 1;
            participants[i].PointsForStanding = currentPoints;
            lastScore = participants[i].TournamentScore;
        }

        return participants;
    }

    private static int CalculateDailyPoints(WordGuess dailyGuess)
    {
        int points = dailyGuess.Guesses.Count; // Each guess counts as a point

        bool isSuccessfulTryExsists = dailyGuess.Guesses
            .Any(g => g.LetterDetails.All(ld => ld.LetterStatus == LetterStatus.Correct));

        if (!isSuccessfulTryExsists)
        {
            points += 1; // Additional point for 6 unsuccessful guesses
        }

        return points;
    }
}
