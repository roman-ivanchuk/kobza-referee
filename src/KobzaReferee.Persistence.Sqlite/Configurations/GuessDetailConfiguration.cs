using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class GuessDetailConfiguration : IEntityTypeConfiguration<GuessDetail>
{
    public void Configure(EntityTypeBuilder<GuessDetail> builder)
    {
        builder.HasKey(gd => gd.Id);

        builder.Property(gd => gd.Id)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(gd => gd.AttemptNumber)
            .IsRequired();

        builder.Property(gd => gd.WordGuessId)
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(gd => gd.LetterDetails)
            .WithOne(ld => ld.GuessDetail)
            .HasForeignKey(ld => ld.GuessDetailId)
            .IsRequired();
    }
}
