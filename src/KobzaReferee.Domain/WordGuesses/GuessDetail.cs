namespace KobzaReferee.Domain.WordGuesses;

public class GuessDetail
{
    public Guid Id { get; set; }

    public int AttemptNumber { get; set; }

    public string WordGuessId { get; set; } = default!;
    public WordGuess WordGuess { get; set; } = default!;

    public ICollection<LetterDetail> LetterDetails { get; set; }
        = new List<LetterDetail>();
}
