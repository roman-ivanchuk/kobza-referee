namespace KobzaReferee.Domain.Entities;

public class GuessDetail
{
    public Guid Id { get; set; }

    public int AttemptNumber { get; set; }

    public List<LetterDetail> LetterDetails { get; set; } = new();
}
