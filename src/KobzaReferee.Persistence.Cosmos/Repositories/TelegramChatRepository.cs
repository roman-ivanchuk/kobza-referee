using System.Linq.Expressions;

namespace KobzaReferee.Persistence.Cosmos.Repositories;

internal class TelegramChatRepository : Repository<TelegramChatCosmosEntity>, ITelegramChatRepository
{
    internal TelegramChatRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }

    public async Task<TelegramChat> CreateAsync(TelegramChat value, CancellationToken cancellationToken = default)
    {
        return await base.CreateAsync(((TelegramChatCosmosEntity)value), cancellationToken);
    }

    public async Task DeleteAsync(TelegramChat value, CancellationToken cancellationToken = default)
    {
        await base.DeleteAsync(((TelegramChatCosmosEntity)value), cancellationToken);
    }

    public Task DeleteRangeAsync(IEnumerable<TelegramChat> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<TelegramChat, TResult>> selector, string? partitionKeyValue = null, Expression<Func<TelegramChat, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TelegramChat>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<TelegramChat, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var result =  await base.GetAllAsync(partitionKeyValue, null, cancellationToken);
        return result.Cast<TelegramChat>().ToList();
    }

    public async Task<TelegramChat> UpsertAsync(TelegramChat value, CancellationToken cancellationToken = default)
    {
        var item = (TelegramChatCosmosEntity)value;
        item.Id = long.Parse(item.id);

        return await base.UpsertAsync(item, cancellationToken);
    }

    Task<TelegramChat?> ITelegramChatRepository.GetByIdAsync(string id, string? partitionKeyValue, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
