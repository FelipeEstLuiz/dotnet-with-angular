namespace Application.Core.DTO.Message;

public record MessageDto
{
    public Guid Id { get; set; }
    public string SenderId { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string? SenderImageUrl { get; set; }
    public string RecipientId { get; set; } = null!;
    public string RecipientName { get; set; } = null!;
    public string? RecipientImageUrl { get; set; }
    public required string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }

    public static MessageDto Map(Domain.Entities.Message message) => new()
    {
        Content = message.Content,
        RecipientId = message.RecipientId,
        DateRead = message.DateRead,
        Id = message.Id,
        MessageSent = message.MessageSent,
        RecipientImageUrl = message.Recipient.Photos?.FirstOrDefault(x => x.IsMain && x.IsApproved)?.Url,
        RecipientName = message.Recipient.UserName!,
        SenderId = message.SenderId,
        SenderImageUrl = message.Sender.Photos?.FirstOrDefault(x => x.IsMain && x.IsApproved)?.Url,
        SenderName = message.Sender.UserName!
    };
}
