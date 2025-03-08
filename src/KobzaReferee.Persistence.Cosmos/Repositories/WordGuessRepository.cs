namespace KobzaReferee.Persistence.Cosmos.Repositories;

internal class WordGuessRepository : Repository<WordGuess>, IWordGuessRepository
{
    internal WordGuessRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }
}
