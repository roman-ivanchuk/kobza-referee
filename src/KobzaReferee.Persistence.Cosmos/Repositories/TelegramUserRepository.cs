using KobzaReferee.Domain.Entities;
using KobzaReferee.Domain.Options;
using KobzaReferee.Persistence.Cosmos.Repositories._Base;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace KobzaReferee.Persistence.Cosmos.Repositories;

public class TelegramUserRepository : Repository<TelegramUser>, ITelegramUserRepository
{
    public TelegramUserRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }
}
