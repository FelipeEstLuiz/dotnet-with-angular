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
        builder.ToTable("aspnet_users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.UserName).HasColumnName("user_name").HasMaxLength(200);
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(150);
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash");
        builder.Property(u => u.NormalizedUserName).HasColumnName("normalized_user_name").HasMaxLength(200);
        builder.Property(u => u.NormalizedEmail).HasColumnName("normalized_email").HasMaxLength(150);
        builder.Property(u => u.Created).HasColumnName("created");
        builder.Property(u => u.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(u => u.KnowAs).HasColumnName("know_as").HasMaxLength(50);
        builder.Property(u => u.LastActive).HasColumnName("last_active");
        builder.Property(u => u.Introduction).HasColumnName("introduction").HasMaxLength(2000);
        builder.Property(u => u.Interests).HasColumnName("interests").HasMaxLength(1000);
        builder.Property(u => u.LookingFor).HasColumnName("looking_for").HasMaxLength(1000);
        builder.Property(u => u.City).HasColumnName("city").HasMaxLength(200);
        builder.Property(u => u.Country).HasColumnName("country").HasMaxLength(50);
        builder.Property(u => u.Gender).HasColumnName("gender").HasMaxLength(50);
        builder.Property(u => u.ImageUrl).HasColumnName("imageUrl").HasMaxLength(500);
    }
}
