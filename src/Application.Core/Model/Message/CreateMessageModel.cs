namespace Application.Core.Model.Message;

public record CreateMessageModel
{
    public int RecipientId { get; set; }
    public required string Content { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int UserId { get; set; }
}
