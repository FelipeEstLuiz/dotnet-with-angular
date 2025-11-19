using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infraestructure.Data.EntitiesConfiguration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("message");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.Property(u => u.Content).HasMaxLength(500);

        builder
            .HasOne(s => s.Recipient)
            .WithMany(t => t.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.Sender)
            .WithMany(t => t.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
