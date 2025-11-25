namespace Application.Core.Model.User;

public record UpdateUserModel
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string? Id { get; set; }


    public string FullName { get; set; } = null!;
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}
