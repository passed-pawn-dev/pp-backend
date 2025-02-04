using System.Security.Claims;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;

namespace PassedPawn.BusinessLogic.Services;

public class PuzzleService(IUnitOfWork unitOfWork, IClaimsPrincipalService claimsPrincipalService) : IPuzzleService
{
    public async Task<ServiceResult<string>> CheckPuzzleSolution(ClaimsPrincipal user, int puzzleId, string puzzleSolution)
    {
        var puzzle = await unitOfWork.Puzzles.GetPuzzleById(puzzleId);

        if (puzzle is null)
            return ServiceResult<string>.Failure(["Puzzle not found"]);

        var student = await claimsPrincipalService.GetStudent(user);

        if (puzzle.Students.Contains(student))
            return ServiceResult<string>.Failure(["Already solved this puzzle"]);

        if (puzzleSolution.Split(',').Length % 2 == 0)
            return ServiceResult<string>.Failure(["Invalid number of moves"]);
        
        if (puzzleSolution == puzzle.Solution)
        {
            student.Puzzles.Add(puzzle);
            unitOfWork.Students.Update(student);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<string>.Success("Puzzle solved successfully");
        }
        
        // e4,Nxb2,e5
        if (puzzle.Solution.StartsWith(puzzleSolution))
        {
            var nextMove = puzzle.Solution
                .Skip(puzzleSolution.Length + 1)
                .TakeWhile(letter => letter != ',')
                .Aggregate(string.Empty, (current, letter) => current + letter);

            return ServiceResult<string>.Success(nextMove!);
        }

        return ServiceResult<string>.Failure(["Invalid solution"]);
    }
}