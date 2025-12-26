namespace Application.Domain.Entities;

public class Photo
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public required string Url { get; set; }
    public string? PublicId { get; set; } = null;
    public bool IsApproved { get; set; }
    public bool IsMain { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}