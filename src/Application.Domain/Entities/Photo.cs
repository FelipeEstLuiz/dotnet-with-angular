namespace Application.Domain.Entities;

public class Photo : Entity
{
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; } = null;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}