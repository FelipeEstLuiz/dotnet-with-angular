namespace Application.Domain.Model;

public record UserLikeParams : QueryOptions
{
    public string Predicate { get; set; } = "liked";
    public int UserId { get; set; }
}
