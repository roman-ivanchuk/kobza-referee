using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TelegramChatConfiguration : IEntityTypeConfiguration<TelegramChat>
{
    public void Configure(EntityTypeBuilder<TelegramChat> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Type)
            .IsRequired();

        builder.HasMany(c => c.WordGuesses)
            .WithOne(wg => wg.Chat)
            .HasForeignKey(wg => wg.ChatId)
            .IsRequired();

        builder.HasMany(c => c.TournamentStatistics)
            .WithOne(wg => wg.Chat)
            .HasForeignKey(wg => wg.ChatId)
            .IsRequired();
    }
}
