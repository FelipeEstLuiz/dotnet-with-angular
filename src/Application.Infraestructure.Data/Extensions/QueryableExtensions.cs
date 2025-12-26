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
        if (options is null)
            return Result.Success(await query.ToListAsync(cancellationToken));

        // Total antes da paginação
        int totalItens = await query.CountAsync(cancellationToken);

        // Ordenação dinâmica
        if (!string.IsNullOrWhiteSpace(options.OrderBy))
        {
            string direction = options.OrderAsc ? "ascending" : "descending";
            query = query.OrderBy($"{options.OrderBy} {direction}");
        }

        // Paginação
        query = query
            .Skip((options.PageNumber - 1) * options.PageSize)
            .Take(options.PageSize);

        List<T> data = await query.ToListAsync(cancellationToken);

        return Result.Success(
            data: data,
            totalItems: totalItens,
            currentPage: options.PageNumber,
            totalPages: (int)Math.Ceiling(totalItens / (double)options.PageSize),
            pageSize: options.PageSize
        );
    }
}
