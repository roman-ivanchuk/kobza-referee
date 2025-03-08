namespace KobzaReferee.Persistence.Cosmos.Repositories;

public class TournamentStatisticsRepository : Repository<TournamentStatistics>, ITournamentStatisticsRepository
{
    public TournamentStatisticsRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }

    protected override string GetPartitionKeyValue(TournamentStatistics value) => value.ChatId;

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
            id: value.Id,
            partitionKey: new PartitionKey(GetPartitionKeyValue(value)),
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
            id: value.Id,
            partitionKey: new PartitionKey(GetPartitionKeyValue(value)),
            patchOperations: patchOperations,
            requestOptions: requestOptions,
            cancellationToken: cancellationToken);

        return response.Resource;
    }
}
