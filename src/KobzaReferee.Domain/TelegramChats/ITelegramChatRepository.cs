using System.Linq.Expressions;

namespace KobzaReferee.Domain.TelegramChats;

public interface ITelegramChatRepository
{
    Task<TelegramChat> CreateAsync(TelegramChat value, CancellationToken cancellationToken = default);
    Task DeleteAsync(TelegramChat value, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TelegramChat> values, CancellationToken cancellationToken = default);
    Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<TelegramChat, TResult>> selector, string? partitionKeyValue = null, Expression<Func<TelegramChat, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default);
    Task<List<TelegramChat>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<TelegramChat, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TelegramChat?> GetByIdAsync(string id, string? partitionKeyValue = null, CancellationToken cancellationToken = default);
    Task<TelegramChat> UpsertAsync(TelegramChat value, CancellationToken cancellationToken = default);
}