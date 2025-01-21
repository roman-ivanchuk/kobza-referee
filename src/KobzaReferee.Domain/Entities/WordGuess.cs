using KobzaReferee.Domain.Entities._Base;

namespace KobzaReferee.Domain.Entities;

public class WordGuess : EntityBase
{
    public string UserId { get; set; } = string.Empty;

    public string ChatId { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public DateTime SubmittedAt { get; set; }

    public List<GuessDetail> Guesses { get; set; } = new();
}
