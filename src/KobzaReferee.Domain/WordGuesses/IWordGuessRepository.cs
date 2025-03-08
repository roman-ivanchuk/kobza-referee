using System.Linq.Expressions;

namespace KobzaReferee.Domain.WordGuesses;

public interface IWordGuessRepository
{
    Task<WordGuess> CreateAsync(WordGuess value, CancellationToken cancellationToken = default);
    Task DeleteAsync(WordGuess value, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<WordGuess> values, CancellationToken cancellationToken = default);
    Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<WordGuess, TResult>> selector, string? partitionKeyValue = null, Expression<Func<WordGuess, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default);
    Task<List<WordGuess>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<WordGuess, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<WordGuess?> GetByIdAsync(string id, string? partitionKeyValue = null, CancellationToken cancellationToken = default);
    Task<WordGuess> UpsertAsync(WordGuess value, CancellationToken cancellationToken = default);
}