namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TelegramChatConfiguration
    : IEntityTypeConfiguration<TelegramChat>
{
    public void Configure(EntityTypeBuilder<TelegramChat> builder)
    {
        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(tc => tc.Type)
            .IsRequired();

        builder.HasMany(tc => tc.WordGuesses)
            .WithOne(wg => wg.Chat)
            .HasForeignKey(wg => wg.ChatId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(tc => tc.TournamentStatistics)
            .WithOne(wg => wg.Chat)
            .HasForeignKey(wg => wg.ChatId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
