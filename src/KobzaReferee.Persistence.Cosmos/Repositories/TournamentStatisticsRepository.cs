using System.Linq.Expressions;

namespace KobzaReferee.Persistence.Cosmos.Repositories;

internal class TournamentStatisticsRepository : Repository<TournamentStatisticsCosmosEntity>, ITournamentStatisticsRepository
{
    internal TournamentStatisticsRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }

    protected override string GetPartitionKeyValue(TournamentStatisticsCosmosEntity value) => value.ChatId.ToString();

    public async Task<TournamentStatistics> SetStandingsChatMessageIdAsync(
        TournamentStatistics value,
        CancellationToken cancellationToken = default)
    {
        var allDailyWordGuessesSubmitted = false;
        if (value.Standings is null || !value.Standings.Any())
        {
            allDailyWordGuessesSubmitted = false;
        }
        else
        {
            var difference = value.EndDate - value.StartDate;
            var tournamentDays = difference.Days + 1; // Adding 1 to include the last day
            allDailyWordGuessesSubmitted = value.Standings.All(tps => tps.ScoreByDate is not null && tps.ScoreByDate.Count == tournamentDays);
        }

        var patchOperations = new List<PatchOperation>()
        {
            PatchOperation.Set($"/{nameof(TournamentStatistics.AllDailyWordGuessesSubmitted)}", allDailyWordGuessesSubmitted),
            PatchOperation.Set($"/{nameof(TournamentStatistics.StandingsChatMessageId)}", value.StandingsChatMessageId)
        };

        var requestOptions = new PatchItemRequestOptions()
        {
            EnableContentResponseOnWrite = true
        };

        var response = await Container.PatchItemAsync<TournamentStatistics>(
            id: value.Id.ToString(),
            partitionKey: new PartitionKey(GetPartitionKeyValue((TournamentStatisticsCosmosEntity)value)),
            patchOperations: patchOperations,
            requestOptions: requestOptions,
            cancellationToken: cancellationToken);

        return response.Resource;
    }

    public async Task<TournamentStatistics> SetSummaryChatMessageIdAsync(
        TournamentStatistics value,
        CancellationToken cancellationToken = default)
    {
        var patchOperations = new List<PatchOperation>()
        {
            PatchOperation.Set($"/{nameof(TournamentStatistics.SummaryChatMessageId)}", value.SummaryChatMessageId)
        };

        var requestOptions = new PatchItemRequestOptions()
        {
            EnableContentResponseOnWrite = true
        };

        var response = await Container.PatchItemAsync<TournamentStatistics>(
            id: value.Id.ToString(),
            partitionKey: new PartitionKey(GetPartitionKeyValue((TournamentStatisticsCosmosEntity)value)),
            patchOperations: patchOperations,
            requestOptions: requestOptions,
            cancellationToken: cancellationToken);

        return response.Resource;
    }

    public Task<TournamentStatistics> CreateAsync(TournamentStatistics value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TournamentStatistics value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<TournamentStatistics> values, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResult>> GetAllAndMapAsync<TResult>(Expression<Func<TournamentStatistics, TResult>> selector, string? partitionKeyValue = null, Expression<Func<TournamentStatistics, bool>>? predicate = null, int? selectCount = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TournamentStatistics>> GetAllAsync(string? partitionKeyValue = null, Expression<Func<TournamentStatistics, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var result = await base.GetAllAsync(partitionKeyValue, null, cancellationToken);
        return result.Cast<TournamentStatistics>().ToList();
    }

    Task<TournamentStatistics?> ITournamentStatisticsRepository.GetByIdAsync(string id, string? partitionKeyValue, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<TournamentStatistics> UpsertAsync(TournamentStatistics value, CancellationToken cancellationToken = default)
    {
        var item = (TournamentStatisticsCosmosEntity)value;

        return await base.UpsertAsync(item, cancellationToken);
    }
}
