using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infraestructure.Data.EntitiesConfiguration;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder
            .HasData(
                new IdentityRole()
                {
                    Id = "user-id",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole()
                {
                    Id = "moderator-id",
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                },
                new IdentityRole()
                {
                    Id = "admin-id",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );
    }
}
