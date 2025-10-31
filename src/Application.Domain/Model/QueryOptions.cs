namespace Application.Domain.Model;

public record QueryOptions
{
    public string? Filtro { get; set; }
    public string? OrdenarPor { get; set; }
    public bool OrdenarAsc { get; set; } = true;
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
}
