namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class GuessDetailConfiguration
    : IEntityTypeConfiguration<GuessDetail>
{
    public void Configure(EntityTypeBuilder<GuessDetail> builder)
    {
        builder.HasKey(gd => gd.Id);

        builder.Property(gd => gd.Id)
            .HasConversion<string>()
            .HasMaxLength(DataSchemaConstants.GUID_LENGTH)
            .IsRequired();

        builder.Property(gd => gd.AttemptNumber)
            .IsRequired();

        builder.Property(gd => gd.WordGuessId)
            .HasMaxLength(DataSchemaConstants.GUID_LENGTH)
            .IsRequired();

        builder.HasMany(gd => gd.LetterDetails)
            .WithOne(ld => ld.GuessDetail)
            .HasForeignKey(ld => ld.GuessDetailId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
