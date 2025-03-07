using KobzaReferee.Api.Middlewares;
using KobzaReferee.Application.Services;
using KobzaReferee.Domain.Options;
using KobzaReferee.Persistence.Cosmos;
using KobzaReferee.Persistence.Sqlite;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

var botConfigurationSection = builder.Configuration.GetSection(BotConfigurationOptions.BotConfiguration);
builder.Services.Configure<BotConfigurationOptions>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfigurationOptions>();
ArgumentNullException.ThrowIfNull(botConfiguration);


builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);

builder.Services.AddHttpClient("telegram_bot_client")
    .RemoveAllLoggers()
    .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
        new TelegramBotClient(botConfiguration.Token, httpClient));

builder.Services.AddTransient<ValidateTelegramBotMiddleware>();

await builder.Services.AddCosmosPersistenceAsync(builder.Configuration);
builder.Services.AddSqlitePersistence();

builder.Services.AddTransient<WordGuessService>();
builder.Services.AddTransient<StandingsService>();

builder.Services.AddScoped<TelegramService>();

//builder.Services.AddHostedService<TelegramWebhookService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<ValidateTelegramBotMiddleware>();

app.MapPost(botConfiguration.WebhookRoute,
    async (TelegramService telegramService,
    ILogger<Program> logger,
    Update update,
    CancellationToken cancellationToken) =>
    {
        if (update.Message is null) return Results.Ok();         // we want only updates about new Message
        if (update.Message.Text is null) return Results.Ok();    // we want only updates about new Text Message

        var handler = update switch
        {
            { Message: { } message } => telegramService.OnMessageReceived(message, cancellationToken),
            _ => Task.CompletedTask
        };

        await handler;

        return Results.Ok();
    }).WithName("TelegramWebhook");

app.Run();
