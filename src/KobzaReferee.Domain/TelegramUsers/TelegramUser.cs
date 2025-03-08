namespace KobzaReferee.Domain.TelegramUsers;

public class TelegramUser : EntityBase
{
    public bool IsBot { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public string? Username { get; set; }

    public string? LanguageCode { get; set; }

    public ICollection<WordGuess> WordGuesses { get; set; }
        = new List<WordGuess>();
    public ICollection<TournamentParticipantStatistics> TournamentParticipantStatistics { get; set; }
        = new List<TournamentParticipantStatistics>();
}
