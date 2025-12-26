namespace Application.Domain.Entities;

public class Group(string name)
{
    public string Name { get; set; } = name;

    // nav property
    public List<Connection> Connections { get; set; } = [];
}
