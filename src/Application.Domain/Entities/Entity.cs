namespace Application.Domain.Entities;

public class Entity
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
