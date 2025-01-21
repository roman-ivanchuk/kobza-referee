namespace KobzaReferee.Domain.Constants;

public static class ChatType
{
    public const string Private = "private";

    public const string Group = "group";

    public const string Supergroup = "supergroup";

    public const string Channel = "channel";

    public static string Validate(string type)
    {
        switch (type.ToLower())
        {
            case Private:
            case Group:
            case Supergroup:
            case Channel:
                return type.ToLower();
            default:
                throw new ArgumentException($"Invalid chat type: {type}");
        }
    }
}
