namespace KobzaReferee.Domain.Statistics;

public class TournamentStatistics
{
    public Guid Id { get; set; }

    /// <summary>
    /// The ID of the chat message that contains the standings of the tournament.
    /// </summary>
    public int? StandingsChatMessageId { get; set; }

    /// <summary>
    /// The ID of the chat message that summarizes the tournament.
    /// </summary>
    public int? SummaryChatMessageId { get; set; }

    /// <summary>
    /// The start date and time of the tournament.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The end date and time of the tournament.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Indicates whether every participant has submitted a word guess for each day of the tournament.
    /// </summary>
    public bool AllDailyWordGuessesSubmitted { get; set; }

    public long ChatId { get; set; }
    public TelegramChat Chat { get; set; } = default!;

    public ICollection<TournamentParticipantStatistics> Standings { get; set; }
        = new List<TournamentParticipantStatistics>();
}
