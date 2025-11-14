namespace Application.Domain.Entities;

public class UserLike
{
    public int SourceUserId { get; set; }
    public User SourceUser { get; set; } = null!;

    public int TargetUserId { get; set; }
    public User TargetUser { get; set; } = null!;
}
