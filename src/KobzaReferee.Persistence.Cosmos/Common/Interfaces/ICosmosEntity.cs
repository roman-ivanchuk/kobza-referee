namespace KobzaReferee.Persistence.Cosmos.Common.Interfaces;

public interface ICosmosEntity
{
    string id { get; }

    string? PartitionKey { get; set; }
}
