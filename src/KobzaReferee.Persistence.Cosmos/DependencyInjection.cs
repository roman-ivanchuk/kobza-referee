using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KobzaReferee.Persistence.Cosmos;

public static class DependencyInjection
{
    public static async Task<IServiceCollection> AddCosmosPersistenceAsync(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var azureCosmosDbAccountSection = configuration.GetSection(AzureCosmosDbAccountOptions.AzureCosmosDbAccount);
        var azureCosmosDbAccount = azureCosmosDbAccountSection.Get<AzureCosmosDbAccountOptions>();
        ArgumentNullException.ThrowIfNull(azureCosmosDbAccount);

        services.Configure<AzureCosmosDbAccountOptions>(azureCosmosDbAccountSection);

        var cosmosClient = await GetCosmosClientAsync(azureCosmosDbAccount);

        services.AddSingleton(cosmosClient);

        services.AddScoped<TelegramChatRepository>();
        services.AddScoped<TelegramUserRepository>();
        services.AddScoped<TournamentStatisticsRepository>();
        services.AddScoped<WordGuessRepository>();

        return services;
    }

    private static async Task<CosmosClient> GetCosmosClientAsync(AzureCosmosDbAccountOptions cosmosDbAccountOptions)
    {
        var cosmosClient = new CosmosClient(
            accountEndpoint: cosmosDbAccountOptions.Endpoint,
            authKeyOrResourceToken: cosmosDbAccountOptions.Key);

        DatabaseResponse databaseResponse;

        if (cosmosDbAccountOptions.Database.AutoscaleMaxThroughput.HasValue)
        {
            var throughputProperties = ThroughputProperties.CreateAutoscaleThroughput(
                cosmosDbAccountOptions.Database.AutoscaleMaxThroughput.Value);

            databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(
                id: cosmosDbAccountOptions.Database.Name,
                throughputProperties: throughputProperties);
        }
        else
        {
            databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(
                cosmosDbAccountOptions.Database.Name);
        }

        foreach (var container in cosmosDbAccountOptions.Database.Containers)
        {
            await EnsureContainerCreatedAsync(container);
        }

        return cosmosClient;

        async Task EnsureContainerCreatedAsync(
            AzureCosmosDbAccountDatabaseContainerOptions containerOptions)
        {
            if (containerOptions is null
                || string.IsNullOrWhiteSpace(containerOptions.Id)
                || string.IsNullOrWhiteSpace(containerOptions.PartitionKey)
                || string.IsNullOrWhiteSpace(containerOptions.Entity))
            {
                throw new ArgumentException($"Unable to parse the container options");
            }

            var containerProperties = new ContainerProperties
            {
                Id = containerOptions.Id,
                PartitionKeyPath = containerOptions.PartitionKey,
                DefaultTimeToLive = containerOptions.DefaultTimeToLive
            };

            if (containerOptions.AutoscaleMaxThroughput.HasValue)
            {
                var throughputProperties = ThroughputProperties.CreateAutoscaleThroughput(
                    containerOptions.AutoscaleMaxThroughput.Value);

                await databaseResponse.Database.CreateContainerIfNotExistsAsync(
                    containerProperties: containerProperties,
                    throughputProperties: throughputProperties);
            }
            else
            {
                await databaseResponse.Database.CreateContainerIfNotExistsAsync(
                    containerProperties: containerProperties);
            }
        }
    }
}
