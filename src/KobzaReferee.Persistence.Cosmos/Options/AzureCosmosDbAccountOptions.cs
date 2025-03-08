namespace KobzaReferee.Persistence.Cosmos.Options;

public class AzureCosmosDbAccountOptions
{
    public static readonly string AzureCosmosDbAccount = "AzureCosmosDbAccount";

    public string Endpoint { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;

    public AzureCosmosDbAccountDatabaseOptions Database { get; set; } = new();
}

public class AzureCosmosDbAccountDatabaseOptions
{
    public string Name { get; set; } = string.Empty;

    public int? AutoscaleMaxThroughput { get; set; }

    public List<AzureCosmosDbAccountDatabaseContainerOptions> Containers { get; set; } = new();
}

public class AzureCosmosDbAccountDatabaseContainerOptions
{
    public string Id { get; set; } = string.Empty;

    public string PartitionKey { get; set; } = string.Empty;

    public string Entity { get; set; } = string.Empty;

    public int? DefaultTimeToLive { get; set; } = -1;

    public int? AutoscaleMaxThroughput { get; set; }
}
