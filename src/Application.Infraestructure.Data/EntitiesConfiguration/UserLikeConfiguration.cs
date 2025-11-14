using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infraestructure.Data.EntitiesConfiguration;

public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
{
    public void Configure(EntityTypeBuilder<UserLike> builder)
    {
        builder.ToTable("user_like");

        builder.HasKey(u => new { u.SourceUserId, u.TargetUserId });

        builder
            .HasOne(s => s.SourceUser)
            .WithMany(t => t.LikedUsers)
            .HasForeignKey(u => u.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.TargetUser)
            .WithMany(t => t.LikedByUsers)
            .HasForeignKey(u => u.TargetUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
