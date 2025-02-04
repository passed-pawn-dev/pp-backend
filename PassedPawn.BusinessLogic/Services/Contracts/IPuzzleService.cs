using System.Security.Claims;
using PassedPawn.Models;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IPuzzleService
{
    Task<ServiceResult<string>> CheckPuzzleSolution(ClaimsPrincipal user, int puzzleId, string puzzleSolution);
}