using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

internal class TelegramUserConfiguration : IEntityTypeConfiguration<TelegramUser>
{
    public void Configure(EntityTypeBuilder<TelegramUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired();

        builder.HasMany(u => u.WordGuesses)
            .WithOne(wg => wg.User)
            .HasForeignKey(wg => wg.UserId);

        builder.HasMany(u => u.TournamentParticipantStatistics)
            .WithOne(wg => wg.User)
            .HasForeignKey(wg => wg.UserId);
    }
}
