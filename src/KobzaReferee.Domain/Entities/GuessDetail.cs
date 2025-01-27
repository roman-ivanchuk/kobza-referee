namespace KobzaReferee.Domain.Entities;

public class GuessDetail
{
    public Guid Id { get; set; }

    public int AttemptNumber { get; set; }

    public Guid WordGuessId { get; set; }
    public WordGuess WordGuess { get; set; } = default!;

    public ICollection<LetterDetail> LetterDetails { get; set; } = new List<LetterDetail>();
}
