namespace KobzaReferee.Persistence.Cosmos;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CosmosClient _cosmosClient;
    private readonly IOptions<AzureCosmosDbAccountOptions> _options;

    private ITelegramChatRepository? _telegramChats;
    private ITelegramUserRepository? _telegramUsers;
    private ITournamentStatisticsRepository? _tournamentStatistics;
    private IWordGuessRepository? _wordGuesses;

    public UnitOfWork(CosmosClient cosmosClient, IOptions<AzureCosmosDbAccountOptions> options)
    {
        _cosmosClient = cosmosClient;
        _options = options;
    }

    public ITelegramChatRepository TelegramChats => _telegramChats
        ??= new TelegramChatRepository(_cosmosClient, _options);
    public ITelegramUserRepository TelegramUsers => _telegramUsers
        ??= new TelegramUserRepository(_cosmosClient, _options);
    public ITournamentStatisticsRepository TournamentStatistics => _tournamentStatistics
        ??= new TournamentStatisticsRepository(_cosmosClient, _options);
    public IWordGuessRepository WordGuesses => _wordGuesses
        ??= new WordGuessRepository(_cosmosClient, _options);

    /// <summary>
    /// In Cosmos DB, write operations are committed immediately. 
    /// This method is a no-op and returns a dummy value to maintain consistency 
    /// with other data store implementations or interfaces.
    /// </summary>
    public int SaveChanges()
    {
        return 1;
    }

    /// <summary>
    /// Asynchronous equivalent of <see cref="SaveChanges"/>. 
    /// Returns a dummy value indicating that no actual save or transaction 
    /// commit was performed, because Cosmos DB operations persist immediately.
    /// </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(1);
    }
}
