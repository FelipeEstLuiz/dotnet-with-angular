namespace Application.Domain.Model;

public record MessageParams : QueryOptions
{
    public int UserId { get; set; }
    public string Container { get; set; } = "Inbox";
}
