namespace KobzaReferee.Domain.Entities;

public class LetterDetail
{
    public Guid Id { get; set; }

    public int Position { get; set; }

    public string LetterStatus { get; set; } = string.Empty;

    public Guid GuessDetailId { get; set; }
    public GuessDetail GuessDetail { get; set; } = default!;
}
