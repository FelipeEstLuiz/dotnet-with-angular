namespace Application.Core.Model.User;

public record BaseUserModel
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Introduction { get; set; }
    public string Gender { get; set; } = null!;
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}
