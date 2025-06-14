using KobzaReferee.Api.Middlewares;
using KobzaReferee.Application.Services;
using KobzaReferee.Infrastructure;
using KobzaReferee.Infrastructure.Options;
using KobzaReferee.Persistence.Cosmos;
using KobzaReferee.Persistence.Sqlite;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

await builder.Services.AddCosmosPersistenceAsync(builder.Configuration);
builder.Services.AddSqlitePersistence();

builder.Services.AddTransient<WordGuessService>();
builder.Services.AddTransient<StandingsService>();

builder.Services.AddScoped<TelegramService>();

builder.Services.AddTransient<ValidateTelegramBotMiddleware>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<ValidateTelegramBotMiddleware>();

var botConfiguration = app.Services.GetRequiredService<IOptions<BotConfigurationOptions>>().Value;

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
