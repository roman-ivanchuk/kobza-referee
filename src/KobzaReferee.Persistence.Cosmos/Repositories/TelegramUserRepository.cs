namespace KobzaReferee.Persistence.Cosmos.Repositories;

internal class TelegramUserRepository : Repository<TelegramUser>, ITelegramUserRepository
{
    internal TelegramUserRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }
}
