namespace Application.Domain.Entities;

public class Photo: Entity
{
    public int PhotoId { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; } = null;
    public int UserId { get; set; }
}