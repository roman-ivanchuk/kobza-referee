using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace KobzaReferee.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var botConfigurationSection = configuration.GetSection(BotConfigurationOptions.BotConfiguration);
        var botConfiguration = botConfigurationSection.Get<BotConfigurationOptions>();
        ArgumentNullException.ThrowIfNull(botConfiguration);

        services.Configure<BotConfigurationOptions>(botConfigurationSection);

        services.ConfigureTelegramBot<JsonOptions>(opt => opt.SerializerOptions);

        services.AddHttpClient("telegram_bot_client")
            .RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
                new TelegramBotClient(botConfiguration.Token, httpClient));

        //builder.Services.AddHostedService<TelegramWebhookService>();

        return services;
    }
}
