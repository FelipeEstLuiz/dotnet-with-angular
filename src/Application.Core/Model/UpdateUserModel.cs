namespace Application.Core.Model;

public record UpdateUserModel : BaseUserModel
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int Id { get; set; }
}
