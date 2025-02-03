using PassedPawn.Models;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IPuzzleService
{
    Task<ServiceResult<string>> CheckPuzzleSolution(string userEmail, int puzzleId, string puzzleSolution);
}