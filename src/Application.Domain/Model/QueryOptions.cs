namespace Application.Domain.Model;

public record QueryOptions
{
    private const int MaxPageSize = 50;

    public string OrderBy { get; set; } = "lastActive";
    public bool OrderAsc { get; set; }
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
