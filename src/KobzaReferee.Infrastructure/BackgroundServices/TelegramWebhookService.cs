using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace KobzaReferee.Infrastructure.BackgroundServices;

public class TelegramWebhookService : IHostedService
{
    private readonly ILogger<TelegramWebhookService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly BotConfigurationOptions _botConfiguration;

    public TelegramWebhookService(
        ILogger<TelegramWebhookService> logger,
        IServiceProvider serviceProvider,
        IOptions<BotConfigurationOptions> botConfigurationOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _botConfiguration = botConfigurationOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhookAddress = $"{_botConfiguration.HostAddress}{_botConfiguration.WebhookRoute}";

        _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);

        try
        {
            await botClient.SetWebhook(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                secretToken: _botConfiguration.SecretToken,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting up the webhook");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        _logger.LogInformation("Removing webhook");

        try
        {
            await botClient.DeleteWebhook(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing the webhook");
        }
    }
}
