using Application.Domain.Model;

namespace Application.Core.Model.User;

public record GetAllUserModel : QueryOptions
{
    public string? Gender { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int? CurrentUserId { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
}
