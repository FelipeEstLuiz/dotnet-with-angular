namespace Application.Domain.Model;

public record MessageParams : QueryOptions
{
    public string UserId { get; set; } = null!;
    public string Container { get; set; } = "Inbox";
}
