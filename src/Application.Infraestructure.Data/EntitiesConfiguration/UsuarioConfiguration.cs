using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Application.Infraestructure.Data.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class UsuarioConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("aspnet_users");

        builder.HasKey(u => u.Id);
        builder.HasKey(u => u.UserId);

        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.UserId).HasColumnName("user_id");
        builder.Property(u => u.UserName).HasColumnName("user_name").HasMaxLength(200);
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(150);
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash");
        builder.Property(u => u.NormalizedUserName).HasColumnName("normalized_user_name").HasMaxLength(200);
        builder.Property(u => u.NormalizedEmail).HasColumnName("normalized_email").HasMaxLength(150);
        builder.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
        builder.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
        builder.Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
        builder.Property(u => u.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
        builder.Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
        builder.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
        builder.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
        builder.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
        builder.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
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
    }
}
