namespace KobzaReferee.Domain.Statistics;

public class TournamentParticipantStatistics
{
    public Guid Id { get; set; }

    /// <summary>
    /// The participant's position in the overall tournament standings.
    /// </summary>
    public int StandingsPosition { get; set; }

    /// <summary>
    /// Points awarded to the participant based on their current standings position.
    /// </summary>
    public int PointsForStanding { get; set; }

    /// <summary>
    /// The participant's total score accumulated during the current tournament.
    /// </summary>
    public int TournamentScore { get; set; }

    /// <summary>
    /// The average time spent by the participant for guessing word.
    /// </summary>
    public TimeSpan AverageGuessTime { get; set; }

    /// <summary>
    /// A record of the participant's daily scores throughout the tournament.
    /// Each entry maps a date to the score achieved on that date.
    /// </summary>
    public Dictionary<DateTime, int> ScoreByDate { get; set; } = new();

    public string UserId { get; set; } = string.Empty;
    public TelegramUser User { get; set; } = default!;

    public string TournamentStatisticsId { get; set; } = string.Empty;
    public TournamentStatistics TournamentStatistics { get; set; } = default!;

    public TournamentParticipantStatistics() { }

    public TournamentParticipantStatistics(string userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }
}
