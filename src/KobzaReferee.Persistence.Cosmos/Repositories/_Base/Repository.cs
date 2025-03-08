using KobzaReferee.Domain.Common.Interfaces;
using KobzaReferee.Persistence.Cosmos.Extensions;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using System.Net;

namespace KobzaReferee.Persistence.Cosmos.Repositories._Base;

internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    internal Repository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
    {
        var cosmosDbAccountOptions = options?.Value;

        ArgumentNullException.ThrowIfNull(cosmosClient);
        ArgumentNullException.ThrowIfNull(cosmosDbAccountOptions);

        var database = cosmosClient.GetDatabase(cosmosDbAccountOptions.Database.Name);

        var entityName = typeof(TEntity).Name;
        var containerOptions = cosmosDbAccountOptions.Database.Containers.FirstOrDefault(f => f.Entity == entityName);
        ArgumentNullException.ThrowIfNull(containerOptions);

        Container = database.GetContainer(containerOptions.Id);
    }

    protected Container Container { get; init; }

    protected string GetDefaultPartitionKeyValue() => typeof(TEntity).Name;
    protected virtual string GetPartitionKeyValue(TEntity value) => typeof(TEntity).Name;

    public virtual async Task<TEntity> CreateAsync(
        TEntity value,
        CancellationToken cancellationToken = default)
    {
        value.PartitionKey = GetPartitionKeyValue(value);

        var response = await Container.CreateItemAsync(
            item: value,
            partitionKey: new PartitionKey(value.PartitionKey),
            cancellationToken: cancellationToken);

        return response.Resource;
    }

    public virtual async Task<TEntity?> GetByIdAsync(
        string id,
        string? partitionKeyValue = null,
        CancellationToken cancellationToken = default)
    {
        var partitionKey = new PartitionKey(partitionKeyValue ?? GetDefaultPartitionKeyValue());

        try
        {
            var response = await Container.ReadItemAsync<TEntity>(
                id: id.ToString(),
                partitionKey: partitionKey,
                cancellationToken: cancellationToken);

            return response.Resource;
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public virtual async Task<List<TEntity>> GetAllAsync(
        string? partitionKeyValue = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var partitionKey = new PartitionKey(partitionKeyValue ?? GetDefaultPartitionKeyValue());

        var queryRequestOptions = new QueryRequestOptions()
        {
            PartitionKey = partitionKey
        };

        var query = Container.GetItemLinqQueryable<TEntity>(requestOptions: queryRequestOptions)
            .AsQueryable();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        using (FeedIterator<TEntity> feedIterator = query.ToFeedIterator())
        {
            return await feedIterator.IterateAsync(cancellationToken);
        }
    }

    public virtual async Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        string? partitionKeyValue = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        int? selectCount = null,
        CancellationToken cancellationToken = default)
    {
        var partitionKey = new PartitionKey(partitionKeyValue ?? GetDefaultPartitionKeyValue());

        var queryRequestOptions = new QueryRequestOptions()
        {
            PartitionKey = partitionKey
        };

        var query = Container.GetItemLinqQueryable<TEntity>(requestOptions: queryRequestOptions)
            .AsQueryable();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (selectCount is not null)
        {
            query = query.Take(selectCount.Value);
        }

        var mappedQuery = query.Select(selector);

        using (FeedIterator<TResult> feedIterator = mappedQuery.ToFeedIterator())
        {
            return await feedIterator.IterateAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> UpsertAsync(
        TEntity value,
        CancellationToken cancellationToken = default)
    {
        value.PartitionKey = GetPartitionKeyValue(value);

        var response = await Container.UpsertItemAsync(
            item: value,
            partitionKey: new PartitionKey(value.PartitionKey),
            cancellationToken: cancellationToken);

        return response.Resource;
    }

    public virtual async Task DeleteAsync(
        TEntity value,
        CancellationToken cancellationToken = default)
    {
        await Container.DeleteItemAsync<TEntity>(
            id: value.Id.ToString(),
            partitionKey: new PartitionKey(GetPartitionKeyValue(value)),
            cancellationToken: cancellationToken);
    }

    public async Task DeleteRangeAsync(
        IEnumerable<TEntity> values,
        CancellationToken cancellationToken = default)
    {
        var partitionKey = values.Select(f => GetPartitionKeyValue(f))
            .Distinct()
            .SingleOrDefault();

        var valuesBatches = values.Chunk(100);

        foreach (var valuesBatch in valuesBatches)
        {
            var tranBatch = Container.CreateTransactionalBatch(new PartitionKey(partitionKey));

            valuesBatch.ToList().ForEach(value =>
            {
                tranBatch.DeleteItem(value.Id.ToString());
            });

            await tranBatch.ExecuteAsync(cancellationToken);
        }
    }
}

public interface IRepository<TEntity> where TEntity : EntityBase;
