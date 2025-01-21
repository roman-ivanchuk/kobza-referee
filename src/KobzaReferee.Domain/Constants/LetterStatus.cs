namespace KobzaReferee.Domain.Constants;

public static class LetterStatus
{
    public const string Correct = "correct";

    public const string Incorrect = "incorrect";

    public const string Misplaced = "misplaced";

    public static string Validate(string type)
    {
        switch (type.ToLower())
        {
            case Correct:
            case Incorrect:
            case Misplaced:
                return type.ToLower();
            default:
                throw new ArgumentException($"Invalid chat type: {type}");
        }
    }
}
