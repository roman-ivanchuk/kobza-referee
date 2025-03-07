namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TournamentStatisticsConfiguration
    : IEntityTypeConfiguration<TournamentStatistics>
{
    public void Configure(EntityTypeBuilder<TournamentStatistics> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Id)
            .HasConversion<string>()
            .HasMaxLength(DataSchemaConstants.GUID_LENGTH)
            .IsRequired();

        builder.Property(ts => ts.StartDate)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(ts => ts.EndDate)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(ts => ts.AllDailyWordGuessesSubmitted)
            .IsRequired();

        builder.Property(ts => ts.ChatId)
            .HasConversion<long>()
            .IsRequired();

        builder.HasMany(ts => ts.Standings)
            .WithOne(tps => tps.TournamentStatistics)
            .HasForeignKey(gd => gd.TournamentStatisticsId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
