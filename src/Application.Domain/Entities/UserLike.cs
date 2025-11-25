namespace Application.Domain.Entities;

public class UserLike
{
    public string SourceUserId { get; set; } = null!;
    public User SourceUser { get; set; } = null!;

    public string TargetUserId { get; set; } = null!;
    public User TargetUser { get; set; } = null!;
}
