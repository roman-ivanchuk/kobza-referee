namespace KobzaReferee.Persistence.Cosmos.Repositories;

internal class TelegramChatRepository : Repository<TelegramChat>, ITelegramChatRepository
{
    internal TelegramChatRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }
}
