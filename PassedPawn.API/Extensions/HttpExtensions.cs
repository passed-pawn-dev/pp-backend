using System.Text.Json;
using PassedPawn.Models;

namespace PassedPawn.API.Extensions;

public static class HttpExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
    {
        response.Headers.Append("Pagination", JsonSerializer.Serialize(header, JsonSerializerOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}
