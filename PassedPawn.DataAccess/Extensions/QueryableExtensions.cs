using Microsoft.EntityFrameworkCore;
using PassedPawn.Models;

namespace PassedPawn.DataAccess.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int currentPage, int pageSize)
    {
        var totalCount = await query.CountAsync();
        List<T> items = await query
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<T>(items, totalCount, currentPage, pageSize);
    }
}