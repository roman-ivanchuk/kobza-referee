using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TournamentStatisticsConfiguration : IEntityTypeConfiguration<TournamentStatistics>
{
    public void Configure(EntityTypeBuilder<TournamentStatistics> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.StandingsChatMessageId)
            .IsRequired();

        builder.Property(ts => ts.SummaryChatMessageId)
            .IsRequired();

        builder.Property(ts => ts.StartDate)
            .IsRequired();

        builder.Property(ts => ts.EndDate)
            .IsRequired();

        builder.Property(ts => ts.AllDailyWordGuessesSubmitted)
            .IsRequired();

        builder.HasMany(ts => ts.Standings)
            .WithOne(tps => tps.TournamentStatistics)
            .HasForeignKey(gd => gd.TournamentStatisticsId)
            .IsRequired();
    }
}
