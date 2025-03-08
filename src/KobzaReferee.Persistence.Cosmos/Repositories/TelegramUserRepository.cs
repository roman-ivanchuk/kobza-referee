namespace KobzaReferee.Persistence.Cosmos.Repositories;

public class TelegramUserRepository : Repository<TelegramUser>, ITelegramUserRepository
{
    public TelegramUserRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }
}
