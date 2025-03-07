namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class WordGuessConfiguration
    : IEntityTypeConfiguration<WordGuess>
{
    public void Configure(EntityTypeBuilder<WordGuess> builder)
    {
        builder.HasKey(wg => wg.Id);

        builder.Property(wg => wg.Id)
            .HasConversion<string>()
            .HasMaxLength(DataSchemaConstants.GUID_LENGTH)
            .IsRequired();

        builder.Property(wg => wg.Date)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(wg => wg.SubmittedAt)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(wg => wg.ChatId)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(wg => wg.UserId)
            .HasConversion<long>()
            .IsRequired();

        builder.HasMany(wg => wg.Guesses)
            .WithOne(gd => gd.WordGuess)
            .HasForeignKey(gd => gd.WordGuessId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
