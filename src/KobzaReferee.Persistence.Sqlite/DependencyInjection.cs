using Microsoft.Extensions.DependencyInjection;

namespace KobzaReferee.Persistence.Sqlite;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlitePersistence(
        this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));

        //services.AddScoped<TelegramChatRepository>();
        //services.AddScoped<TelegramUserRepository>();
        //services.AddScoped<TournamentStatisticsRepository>();
        //services.AddScoped<WordGuessRepository>();

        return services;
    }
}
