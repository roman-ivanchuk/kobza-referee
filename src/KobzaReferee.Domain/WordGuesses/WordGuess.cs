namespace KobzaReferee.Domain.WordGuesses;

public class WordGuess
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public DateTime SubmittedAt { get; set; }

    public long UserId { get; set; }
    public TelegramUser User { get; set; } = default!;

    public long ChatId { get; set; }
    public TelegramChat Chat { get; set; } = default!;

    public ICollection<GuessDetail> Guesses { get; set; }
        = new List<GuessDetail>();
}
