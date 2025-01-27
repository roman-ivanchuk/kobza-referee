using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TournamentParticipantStatisticsConfiguration : IEntityTypeConfiguration<TournamentParticipantStatistics>
{
    public void Configure(EntityTypeBuilder<TournamentParticipantStatistics> builder)
    {
        builder.HasKey(tps => tps.Id);

        builder.Property(tps => tps.StandingsPosition)
            .IsRequired();

        builder.Property(tps => tps.PointsForStanding)
            .IsRequired();

        builder.Property(tps => tps.AverageGuessTime)
            .IsRequired();

        builder.Property(tps => tps.TournamentScore)
            .IsRequired();

        builder.Property(tps => tps.ScoreByDate)
            .IsRequired();
    }
}
