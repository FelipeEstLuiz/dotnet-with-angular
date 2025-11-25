namespace Application.Domain.Entities;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;

    public string SenderId { get; set; } = null!;
    public User Sender { get; set; } = null!;
    public string RecipientId { get; set; } = null!;
    public User Recipient { get; set; } = null!;
}
