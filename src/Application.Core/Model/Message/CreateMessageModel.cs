namespace Application.Core.Model.Message;

public record CreateMessageModel
{
    public string RecipientId { get; set; } = null!;
    public required string Content { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string UserId { get; set; } = null!;
    public DateTime? DateRead { get; set; }
}
