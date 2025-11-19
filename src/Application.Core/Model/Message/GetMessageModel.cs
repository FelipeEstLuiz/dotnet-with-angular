using Application.Domain.Model;

namespace Application.Core.Model.Message;

public record GetMessageModel : QueryOptions
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int UserId { get; set; }
    public string Container { get; set; } = "Inbox";
}
