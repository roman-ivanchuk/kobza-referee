namespace KobzaReferee.Persistence.Cosmos.Extensions;

internal static class CosmosQueryableExtensions
{
    public static async Task<List<TItem>> IterateAsync<TItem>(
        this FeedIterator<TItem> feedIterator,
        CancellationToken cancellationToken = default)
    {
        List<TItem> results = new();

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<TItem> feedResponse = await feedIterator
                .ReadNextAsync(cancellationToken);

            results.AddRange(feedResponse.Resource);
        }

        return results;
    }
}
