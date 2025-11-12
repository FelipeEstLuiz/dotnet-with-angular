namespace Application.Domain.Model;

public record UserParams : QueryOptions
{
    public string? Gender { get; set; }

    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int? CurrentUserId { get; set; }
}
