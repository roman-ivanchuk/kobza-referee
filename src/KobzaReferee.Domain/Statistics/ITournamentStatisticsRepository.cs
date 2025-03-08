using System.Linq.Expressions;

namespace KobzaReferee.Domain.Statistics;

public interface ITournamentStatisticsRepository
{
    Task<TournamentStatistics> CreateAsync(TournamentStatistics value, CancellationToken cancellationToken = default);
    Task DeleteAsync(TournamentStatistics value, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TournamentStatistics> values, CancellationToken cancellationToken = default);
    Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<TournamentStatistics, TResult>> selector, string? partitionKeyValue = null, Expression<Func<TournamentStatistics, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default);
    Task<List<TournamentStatistics>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<TournamentStatistics, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TournamentStatistics?> GetByIdAsync(string id, string? partitionKeyValue = null, CancellationToken cancellationToken = default);
    Task<TournamentStatistics> UpsertAsync(TournamentStatistics value, CancellationToken cancellationToken = default);
    Task<TournamentStatistics> SetStandingsChatMessageIdAsync(TournamentStatistics value, CancellationToken cancellationToken = default);
    Task<TournamentStatistics> SetSummaryChatMessageIdAsync(TournamentStatistics value, CancellationToken cancellationToken = default);
}