using Microsoft.AspNetCore.Http;
using PassedPawn.Models;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IHttpService
{
    void AddPaginationHeader(HttpResponse response, PaginationHeader header);
}
