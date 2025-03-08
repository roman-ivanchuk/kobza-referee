namespace KobzaReferee.Persistence.Cosmos.Repositories;

public class TelegramChatRepository : Repository<TelegramChat>, ITelegramChatRepository
{
    public TelegramChatRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }
}
