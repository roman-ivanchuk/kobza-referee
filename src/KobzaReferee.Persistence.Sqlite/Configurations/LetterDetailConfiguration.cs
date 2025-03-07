using KobzaReferee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KobzaReferee.Persistence.Sqlite.Configurations;

public class LetterDetailConfiguration : IEntityTypeConfiguration<LetterDetail>
{
    public void Configure(EntityTypeBuilder<LetterDetail> builder)
    {
        builder.HasKey(ld => ld.Id);

        builder.Property(ld => ld.Id)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(ld => ld.Position)
            .IsRequired();

        builder.Property(ld => ld.LetterStatus)
            .IsRequired();

        builder.Property(ld => ld.GuessDetailId)
            .HasConversion<string>()
            .IsRequired();
    }
}
