using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Application.Infraestructure.Data.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.UserName).HasMaxLength(200);
        builder.Property(u => u.FullName).HasMaxLength(200);
        builder.Property(u => u.Email).HasMaxLength(150);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(200);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(150);
        builder.Property(u => u.Introduction).HasMaxLength(2000);
        builder.Property(u => u.Interests).HasMaxLength(1000);
        builder.Property(u => u.LookingFor).HasMaxLength(1000);
        builder.Property(u => u.City).HasMaxLength(200);
        builder.Property(u => u.Country).HasMaxLength(50);
        builder.Property(u => u.Gender).HasMaxLength(50);
        builder.Property(u => u.ImageUrl).HasMaxLength(500);
    }
}
