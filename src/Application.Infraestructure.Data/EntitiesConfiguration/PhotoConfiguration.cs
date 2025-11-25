using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Application.Infraestructure.Data.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Url).HasMaxLength(500);
        builder.Property(u => u.PublicId).HasMaxLength(500);

        builder
            .HasOne(p => p.User)
            .WithMany(u => u.Photos)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
