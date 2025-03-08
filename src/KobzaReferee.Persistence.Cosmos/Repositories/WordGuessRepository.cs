namespace KobzaReferee.Persistence.Cosmos.Repositories;

public class WordGuessRepository : Repository<WordGuess>, IWordGuessRepository
{
    public WordGuessRepository(
        CosmosClient cosmosClient,
        IOptions<AzureCosmosDbAccountOptions> options)
        : base(cosmosClient, options) { }
}
