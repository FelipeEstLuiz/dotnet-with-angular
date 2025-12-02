using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infraestructure.Data.EntitiesConfiguration;

public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.HasKey(u => u.ConnectionId);

        builder
            .HasOne(p => p.Group)
            .WithMany(u => u.Connections)
            .HasForeignKey(p => p.GroupName)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
