namespace Application.Domain.VO;

public class UserVo
{
    public int Id { get;  set; }
    public DateTime Created { get;  set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public DateTime LastActive { get; set; }
    public int Age { get; set; }
    public string KnowAs { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl => Photos?.FirstOrDefault(x => x.IsMain)?.Url;
    public List<PhotoVo>? Photos { get; set; }
}
