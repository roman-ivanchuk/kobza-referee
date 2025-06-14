namespace KobzaReferee.Persistence.Cosmos.Entities;

public class TournamentStatisticsCosmosEntity : TournamentStatistics, ICosmosEntity
{
    public string id => Id.ToString();

    public string? PartitionKey { get; set; }
}
