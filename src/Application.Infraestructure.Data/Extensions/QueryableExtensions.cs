using Application.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Infraestructure.Data.Extensions;

public static class QueryableExtensions
{
    public static async Task<Result<List<T>>> ApplyQueryOptionsAsync<T>(
        this IQueryable<T> query,
        QueryOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        if(options is null)
            return Result<List<T>>.Success(await query.ToListAsync(cancellationToken));

        // Total antes da paginação
        int totalItens = await query.CountAsync(cancellationToken);

        // Ordenação dinâmica
        if (!string.IsNullOrWhiteSpace(options.OrdenarPor))
        {
            string direcao = options.OrdenarAsc ? "ascending" : "descending";
            query = query.OrderBy($"{options.OrdenarPor} {direcao}");
        }

        // Paginação
        query = query
            .Skip((options.Pagina - 1) * options.TamanhoPagina)
            .Take(options.TamanhoPagina);

        List<T> data = await query.ToListAsync(cancellationToken);

        return Result<List<T>>.Success(
            data: data,
            totalItens: totalItens,
            paginaAtual: options.Pagina,
            totalPaginas: (int)Math.Ceiling(totalItens / (double)options.TamanhoPagina)
        );
    }
}
