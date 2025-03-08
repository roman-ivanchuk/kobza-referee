namespace KobzaReferee.Domain.Common.Interfaces;

public abstract class EntityBase
{
    //[JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    public string? PartitionKey { get; set; }
}
