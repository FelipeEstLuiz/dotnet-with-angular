using Application.Domain.Model;

namespace Application.Core.Model.Like;

public record GetUserLikesModel : QueryOptions
{
    public string Predicate { get; set; } = "liked";

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string UserId { get; set; } = null!;
}
