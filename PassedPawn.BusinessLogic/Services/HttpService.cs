using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.Models;

namespace PassedPawn.BusinessLogic.Services;

public class HttpService : IHttpService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public void AddPaginationHeader(HttpResponse response, PaginationHeader header)
    {
        response.Headers.Append("Pagination", JsonSerializer.Serialize(header, JsonSerializerOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }
}