using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICoursePuzzleRepository : IRepositoryBase<CoursePuzzle>
{
    Task<CoursePuzzle?> GetPuzzleById(int id);
}