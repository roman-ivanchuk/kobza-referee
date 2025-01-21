using KobzaReferee.Domain.Options;
using Microsoft.Extensions.Options;

namespace KobzaReferee.Api.Middlewares;

public sealed class ValidateTelegramBotMiddleware : IMiddleware
{
    private readonly BotConfigurationOptions _botConfiguration;

    public ValidateTelegramBotMiddleware(IOptions<BotConfigurationOptions> botConfigurationOptions)
    {
        _botConfiguration = botConfigurationOptions.Value;
    }

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments(_botConfiguration.WebhookRoute))
        {
            if (!IsValidRequest(context.Request))
            {
                context.Response.StatusCode = 403;

                await context.Response.WriteAsync("\"X-Telegram-Bot-Api-Secret-Token\" is invalid");

                return;
            }
        }

        await next(context);
    }

    private bool IsValidRequest(HttpRequest request)
    {
        if (request.Headers.TryGetValue("X-Telegram-Bot-Api-Secret-Token", out var secretTokenHeader))
        {
            return string.Equals(
                secretTokenHeader,
                _botConfiguration.SecretToken,
                StringComparison.Ordinal);
        }

        return false;
    }
}
