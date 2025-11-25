namespace Application.Domain.Model;

public record UserLikeParams : QueryOptions
{
    public string Predicate { get; set; } = "liked";
    public string UserId { get; set; } = null!;
}
