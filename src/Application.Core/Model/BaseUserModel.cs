namespace Application.Core.Model;

public record BaseUserModel
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;   
    public string? Introduction { get; set; }
    public string Gender { get; set; } = null!;
    public string KnowAs { get; set; } = null!;
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
