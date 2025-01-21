namespace KobzaReferee.Domain.Entities._Base;

public abstract class EntityBase
{
    //[JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    public string? PartitionKey { get; set; }
}
