namespace Application.Domain.Entities;

public class Entity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
