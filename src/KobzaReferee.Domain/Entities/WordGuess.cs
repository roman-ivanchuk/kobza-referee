using KobzaReferee.Domain.Entities._Base;

namespace KobzaReferee.Domain.Entities;

public class WordGuess : EntityBase
{
    public DateTime Date { get; set; }

    public DateTime SubmittedAt { get; set; }

    public string UserId { get; set; } = string.Empty;
    public TelegramUser User { get; set; } = default!;

    public string ChatId { get; set; } = string.Empty;
    public TelegramChat Chat { get; set; } = default!;

    public ICollection<GuessDetail> Guesses { get; set; } = new List<GuessDetail>();
}
