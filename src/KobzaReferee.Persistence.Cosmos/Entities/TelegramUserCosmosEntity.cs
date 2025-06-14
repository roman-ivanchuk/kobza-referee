namespace KobzaReferee.Persistence.Cosmos.Entities;

public class TelegramUserCosmosEntity : TelegramUser, ICosmosEntity
{
    public string id => Id.ToString();

    public string? PartitionKey { get; set; }
}
