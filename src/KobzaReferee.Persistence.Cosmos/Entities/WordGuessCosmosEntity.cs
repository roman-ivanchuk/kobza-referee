namespace KobzaReferee.Persistence.Cosmos.Entities;

public class WordGuessCosmosEntity : WordGuess, ICosmosEntity
{
    public string id => Id.ToString();

    public string? PartitionKey { get; set; }
}
