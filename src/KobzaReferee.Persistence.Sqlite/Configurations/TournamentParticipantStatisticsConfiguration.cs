using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TournamentParticipantStatisticsConfiguration : IEntityTypeConfiguration<TournamentParticipantStatistics>
{
    public void Configure(EntityTypeBuilder<TournamentParticipantStatistics> builder)
    {
        builder.HasKey(tps => tps.Id);

        builder.Property(tps => tps.Id)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tps => tps.StandingsPosition)
            .IsRequired();

        builder.Property(tps => tps.PointsForStanding)
            .IsRequired();

        builder.Property(tps => tps.AverageGuessTime)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(tps => tps.TournamentScore)
            .IsRequired();

        builder.Property(tps => tps.UserId)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tps => tps.TournamentStatisticsId)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tps => tps.ScoreByDate)
            .HasConversion(sbd => Serialize(sbd), sbd => Deserialize(sbd))
            .IsRequired();
    }

    private string Serialize(Dictionary<DateTime, int> value)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
        var result = JsonSerializer.Serialize(value, options);
        return result;
    }

    private Dictionary<DateTime, int> Deserialize(string json)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
        var result = JsonSerializer.Deserialize<Dictionary<DateTime, int>>(json, options);
        return result ?? new();
    }
}
