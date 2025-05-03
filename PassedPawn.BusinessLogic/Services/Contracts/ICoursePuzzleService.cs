using System.Security.Claims;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Puzzle;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICoursePuzzleService
{
    public Task<ServiceResult<CoursePuzzlesDto>> ValidateAndAddPuzzle(Lesson lesson, CoursePuzzleUpsertDto upsertDto);

    public Task<ServiceResult<CoursePuzzlesDto>> ValidateAndUpdatePuzzle(Lesson lesson, int exampleId,
        CoursePuzzleUpsertDto upsertDto);

    public Task DeletePuzzle(Lesson lesson, CoursePuzzle coursePuzzle);
    
    Task<ServiceResult<string>> CheckPuzzleSolution(ClaimsPrincipal user, int puzzleId, string puzzleSolution);
}
