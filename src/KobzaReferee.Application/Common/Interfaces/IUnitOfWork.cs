namespace KobzaReferee.Application.Common.Interfaces;

public interface IUnitOfWork
{
    ITelegramChatRepository TelegramChats { get; }
    ITelegramUserRepository TelegramUsers { get; }
    ITournamentStatisticsRepository TournamentStatistics { get; }
    IWordGuessRepository WordGuesses { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
