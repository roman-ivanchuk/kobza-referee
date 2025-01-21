using KobzaReferee.Application.Dtos;
using KobzaReferee.Domain.Entities;
using System.Globalization;
using System.Text;

namespace KobzaReferee.Application.Helpers;

internal static class TelegramMessageBuilder
{
    private static readonly CultureInfo UkrainianCulture = new CultureInfo("uk-UA");

    public static string BuildSummaryMessage(
        TournamentStatistics tournamentStatistics,
        List<OverallParticipantStatisticsDto> overallParticipantStatistics,
        List<TelegramUser> users)
    {
        CultureInfo UkrainianCulture = new CultureInfo("uk-UA");

        if (tournamentStatistics.Standings == null || !tournamentStatistics.Standings.Any())
        {
            return "Tournament Standings not found";
        }

        var winnerTournamentScore = tournamentStatistics.Standings.Min(tps => tps.TournamentScore);
        var winners = tournamentStatistics.Standings.Where(tps => tps.TournamentScore == winnerTournamentScore).ToList();
        var message = new StringBuilder();

        string dateRange = $"{tournamentStatistics.StartDate.ToString("dd MMM", UkrainianCulture)} - {tournamentStatistics.EndDate.ToString("dd MMM", UkrainianCulture)}";
        message.AppendLine(winners.Count == 1
            ? $"🏆 Визначився переможець чемпіонату за тиждень: {dateRange} 🏆"
            : $"🏆 Визначилися переможці чемпіонату за тиждень: {dateRange} 🏆");

        message.AppendLine();
        if (winners.Count == 1)
        {
            var winner = winners.Single();
            var winnerUser = users.FirstOrDefault(tu => tu.Id == winner.UserId);
            if (winnerUser != null)
            {
                message.Append($"Новим переможцем стає: {winnerUser.FirstName} {winnerUser.LastName ?? string.Empty}! ");
                var winnerOverallStatistics = overallParticipantStatistics.FirstOrDefault(ops => ops.UserId == winner.UserId);
                if (winnerOverallStatistics != null)
                {
                    message.Append($"Вітаємо Вас з {winnerOverallStatistics.TotalWins} перемогою! 🏆");
                }
            }
            message.AppendLine();
        }
        else
        {
            message.AppendLine($"Новим переможцями стають:");
            var sortedWinners = winners.OrderBy(f => f.AverageGuessTime).ToList();
            foreach (var winner in sortedWinners)
            {
                var winnerUser = users.FirstOrDefault(u => u.Id == winner.UserId);
                if (winnerUser != null)
                {
                    message.Append($"- {winnerUser.FirstName} {winnerUser.LastName ?? string.Empty}! ");
                    var winnerOverallStatistics = overallParticipantStatistics.FirstOrDefault(ops => ops.UserId == winner.UserId);
                    if (winnerOverallStatistics != null)
                    {
                        message.Append($"Вітаємо Вас з {winnerOverallStatistics.TotalWins} перемогою! 🏆");
                    }
                }

                message.AppendLine();
            }
        }

        message.AppendLine();

        message.AppendLine("📊 Таблиця лідерів:");
        var standings = overallParticipantStatistics.OrderByDescending(ops => ops.TotalWins).ThenBy(f => f.AveragePointsForStanding).ToList();
        foreach (var participant in standings)
        {
            var user = users.FirstOrDefault(u => u.Id == participant.UserId);
            var position = standings.IndexOf(participant) + 1;
            message.AppendLine($"{position}. {user?.FirstName} {user?.LastName ?? string.Empty}: {participant.TotalWins} 🏆");
        }

        message.AppendLine();

        message.AppendLine("Дякуємо всім за участь! Бажаємо удачі у наступному турнірі!");

        return message.ToString();
    }

    public static string BuildStandingsMessage(
        List<TelegramUser> users,
        TournamentStatistics tournamentStatistics)
    {
        var message = new StringBuilder($"🏆 Результати чемпіонату за тиждень: {tournamentStatistics.StartDate.ToString("dd MMM", UkrainianCulture)} - {tournamentStatistics.EndDate.ToString("dd MMM", UkrainianCulture)} 🏆\n");
        message.AppendLine("------------------------------");

        var utcNow = DateTime.UtcNow;
        var ukrainianZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
        var ukrainianNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, ukrainianZone);
        var standings = tournamentStatistics.Standings.OrderBy(tps => tps.StandingsPosition).ToList();

        foreach (var participant in standings)
        {
            var user = users.FirstOrDefault(f => f.Id == participant.UserId);

            if (user is null)
            {
                continue;
            }

            message.AppendLine($"{participant.PointsForStanding}. {user.FirstName} {user.LastName ?? string.Empty}: {participant.TournamentScore} 🌟");
            message.Append("📊 По днях: ");

            for (var day = tournamentStatistics.StartDate.Date; day.Date <= tournamentStatistics.EndDate.Date; day = day.AddDays(1))
            {
                // Do not show dates greater than today
                if (day.Date > ukrainianNow.Date)
                {
                    continue;
                }

                var ukrainianDayName = UkrainianCulture.DateTimeFormat.GetAbbreviatedDayName(day.DayOfWeek).ToUpper();

                var dailyPoint = participant.ScoreByDate.ContainsKey(day)
                    ? participant.ScoreByDate[day].ToString()
                    : "❌";

                message.Append($"{ukrainianDayName}: {dailyPoint} | ");
            }

            // Removing the trailing " | "
            message.Length -= 3;
            message.AppendLine("\n");
        }

        return message.ToString();
    }
}
