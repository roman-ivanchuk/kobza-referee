﻿using KobzaReferee.Domain.Entities._Base;

namespace KobzaReferee.Domain.Entities;

public class TournamentStatistics : EntityBase
{
    public string ChatId { get; set; } = string.Empty;

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

    /// <summary>
    /// The standings of participants in the tournament.
    /// </summary>
    public List<TournamentParticipantStatistics> Standings { get; set; } = new();

    public TournamentStatistics() { }

    public TournamentStatistics(string chatId)
    {
        Id = Guid.NewGuid().ToString();
        ChatId = chatId;
    }
}