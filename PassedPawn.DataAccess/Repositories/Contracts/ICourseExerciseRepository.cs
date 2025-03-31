using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseExerciseRepository : IRepositoryBase<CourseExercise>
{
    Task<CourseExercise?> GetPuzzleById(int id);
}