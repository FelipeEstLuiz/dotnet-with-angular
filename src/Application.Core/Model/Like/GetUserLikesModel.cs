using Application.Domain.Model;

namespace Application.Core.Model.Like;

public record GetUserLikesModel : QueryOptions
{
    public string Predicate { get; set; } = "liked";
    public int UserId { get; set; }
}
