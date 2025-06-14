using System.Linq.Expressions;

namespace KobzaReferee.Persistence.Cosmos.Repositories;

internal class WordGuessRepository : Repository<WordGuessCosmosEntity>, IWordGuessRepository
{
    internal WordGuessRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }

    public Task<WordGuess> CreateAsync(WordGuess value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(WordGuess value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<WordGuess> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<WordGuess, TResult>> selector, string? partitionKeyValue = null, Expression<Func<WordGuess, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<WordGuess>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<WordGuess, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAllAsync(partitionKeyValue, null, cancellationToken);
        return result.Cast<WordGuess>().ToList();
    }

    public async Task<WordGuess> UpsertAsync(WordGuess value, CancellationToken cancellationToken = default)
    {
        var item = (WordGuessCosmosEntity)value;

        return await base.UpsertAsync(item, cancellationToken);
    }

    Task<WordGuess?> IWordGuessRepository.GetByIdAsync(string id, string? partitionKeyValue, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
