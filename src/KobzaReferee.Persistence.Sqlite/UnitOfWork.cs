namespace KobzaReferee.Persistence.Sqlite;

internal class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ITelegramChatRepository? _telegramChats;
    private ITelegramUserRepository? _telegramUsers;
    private ITournamentStatisticsRepository? _tournamentStatistics;
    private IWordGuessRepository? _wordGuesses;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ITelegramChatRepository TelegramChats => _telegramChats;
    //??= new TelegramChatRepository(_cosmosClient, _options);
    public ITelegramUserRepository TelegramUsers => _telegramUsers;
    //??= new TelegramUserRepository(_cosmosClient, _options);
    public ITournamentStatisticsRepository TournamentStatistics => _tournamentStatistics;
    //??= new TournamentStatisticsRepository(_cosmosClient, _options);
    public IWordGuessRepository WordGuesses => _wordGuesses;
    //??= new WordGuessRepository(_cosmosClient, _options);

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
