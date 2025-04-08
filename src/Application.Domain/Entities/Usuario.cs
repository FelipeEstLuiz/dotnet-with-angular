namespace Application.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public string User_Name { get; set; } = string.Empty;
    public string Normalized_User_Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Normalized_Email { get; set; } = string.Empty;
    public bool Email_Confirmed { get; set; }
    public string Password_Hash { get; set; } = string.Empty;
    public string Security_Stamp { get; set; } = Guid.NewGuid().ToString();
    public string Concurrency_Stamp { get; set; } = Guid.NewGuid().ToString();
    public string? Phone_Number { get; set; }
    public bool Phone_Number_Confirmed { get; set; }
    public bool Two_Factor_Enabled { get; set; }
    public DateTimeOffset? Lockout_End { get; set; }
    public bool Lockout_Enabled { get; set; }
    public int Access_Failed_Count { get; set; }
    public DateTime Criado_Em { get; set; } = DateTime.UtcNow;


    public static Usuario Create(string nome, string email) => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        Normalized_Email = email.ToUpperInvariant(),
        User_Name = nome,
        Normalized_User_Name = nome.ToUpperInvariant()
    };
}
