using System.Linq.Expressions;

namespace KobzaReferee.Domain.TelegramUsers;

public interface ITelegramUserRepository
{
    Task<TelegramUser> CreateAsync(TelegramUser value, CancellationToken cancellationToken = default);
    Task DeleteAsync(TelegramUser value, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TelegramUser> values, CancellationToken cancellationToken = default);
    Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<TelegramUser, TResult>> selector, string? partitionKeyValue = null, Expression<Func<TelegramUser, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default);
    Task<List<TelegramUser>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<TelegramUser, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TelegramUser?> GetByIdAsync(string id, string? partitionKeyValue = null, CancellationToken cancellationToken = default);
    Task<TelegramUser> UpsertAsync(TelegramUser value, CancellationToken cancellationToken = default);
}