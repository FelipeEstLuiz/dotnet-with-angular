namespace Application.Core.DTO;

public record ComumDto
{
    public string Id { get; internal set; } = null!;

    public DateTime Created { get; internal set; }
}
