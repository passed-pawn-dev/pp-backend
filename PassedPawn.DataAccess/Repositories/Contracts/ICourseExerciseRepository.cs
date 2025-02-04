using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseExerciseRepository : IRepositoryBase<CourseExercise>
{
    Task<CourseExercise?> GetPuzzleById(int id);
}