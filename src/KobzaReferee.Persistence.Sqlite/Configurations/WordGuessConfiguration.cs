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
            .IsRequired();

        builder.Property(wg => wg.SubmittedAt)
            .IsRequired();

        builder.HasMany(wg => wg.Guesses)
            .WithOne(gd => gd.WordGuess)
            .HasForeignKey(gd => gd.WordGuessId)
            .IsRequired();
    }
}
