namespace KobzaReferee.Domain.Options;

public class BotConfigurationOptions
{
    public static readonly string BotConfiguration = "BotConfiguration";

    public string Token { get; init; } = default!;

    public string HostAddress { get; init; } = default!;

    public string WebhookRoute { get; init; } = default!;

    public string SecretToken { get; init; } = default!;
}
