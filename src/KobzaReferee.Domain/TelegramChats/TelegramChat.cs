namespace KobzaReferee.Domain.TelegramChats;

public class TelegramChat : EntityBase
{
    public string Type { get; set; } = string.Empty;

    public string? Title { get; set; }

    public string? Username { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool? IsForum { get; set; }

    public ICollection<WordGuess> WordGuesses { get; set; }
        = new List<WordGuess>();
    public ICollection<TournamentStatistics> TournamentStatistics { get; set; }
        = new List<TournamentStatistics>();
}
