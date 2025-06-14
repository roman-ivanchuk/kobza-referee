using System.Linq.Expressions;

namespace KobzaReferee.Persistence.Cosmos.Repositories;

internal class TelegramUserRepository : Repository<TelegramUserCosmosEntity>, ITelegramUserRepository
{
    internal TelegramUserRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }

    public Task<TelegramUser> CreateAsync(TelegramUser value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TelegramUser value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<TelegramUser> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<TelegramUser, TResult>> selector, string? partitionKeyValue = null, Expression<Func<TelegramUser, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TelegramUser>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<TelegramUser, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAllAsync(partitionKeyValue, null, cancellationToken);
        return result.Cast<TelegramUser>().ToList();
    }

    public async Task<TelegramUser> UpsertAsync(TelegramUser value, CancellationToken cancellationToken = default)
    {
        var item = (TelegramUserCosmosEntity)value;
        item.Id = long.Parse(item.id);

        return await base.UpsertAsync(item, cancellationToken);
    }

    Task<TelegramUser?> ITelegramUserRepository.GetByIdAsync(string id, string? partitionKeyValue, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
