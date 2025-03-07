namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TelegramUserConfiguration
    : IEntityTypeConfiguration<TelegramUser>
{
    public void Configure(EntityTypeBuilder<TelegramUser> builder)
    {
        builder.HasKey(tu => tu.Id);

        builder.Property(tu => tu.Id)
            .ValueGeneratedNever()
            .HasConversion<long>()
            .IsRequired();

        builder.Property(tu => tu.FirstName)
            .IsRequired();

        builder.HasMany(tu => tu.WordGuesses)
            .WithOne(wg => wg.User)
            .HasForeignKey(wg => wg.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(tu => tu.TournamentParticipantStatistics)
            .WithOne(wg => wg.User)
            .HasForeignKey(wg => wg.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
