using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.DTOs.Course.Puzzle;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICoursePuzzleRepository : IRepositoryBase<CoursePuzzle>
{
    Task<CoursePuzzlesDto?> GetOwnedOrInPreviewForStudentAsync(int puzzleId, int userId);
    Task<CoursePuzzlesDto?> GetOwnedOrInPreviewForCoachAsync(int puzzleId, int userId);
    Task<CoursePuzzle?> GetPuzzleById(int id);
}