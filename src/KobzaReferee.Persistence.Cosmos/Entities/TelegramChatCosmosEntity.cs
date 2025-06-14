namespace KobzaReferee.Persistence.Cosmos.Entities;

public class TelegramChatCosmosEntity : TelegramChat, ICosmosEntity
{
    public string id => Id.ToString();

    public string? PartitionKey { get; set; }
}
