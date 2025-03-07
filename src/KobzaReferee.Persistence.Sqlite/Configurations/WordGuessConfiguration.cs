using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class WordGuessConfiguration : IEntityTypeConfiguration<WordGuess>
{
    public void Configure(EntityTypeBuilder<WordGuess> builder)
    {
        builder.HasKey(wg => wg.Id);

        builder.Property(wg => wg.Date)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(wg => wg.SubmittedAt)
            .HasConversion<long>()
            .IsRequired();

        builder.Property(wg => wg.ChatId)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(wg => wg.UserId)
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(wg => wg.Guesses)
            .WithOne(gd => gd.WordGuess)
            .HasForeignKey(gd => gd.WordGuessId)
            .IsRequired();
    }
}
