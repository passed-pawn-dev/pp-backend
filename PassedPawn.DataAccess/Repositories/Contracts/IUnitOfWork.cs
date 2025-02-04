using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IUnitOfWork
{
    IStudentRepository Students { get; }
    ICoachRepository Coaches { get; }
    IRepositoryBase<Nationality> Nationalities { get; }
    ICourseRepository Courses { get; }
    IRepositoryBase<Lesson> Lessons { get; }
    IRepositoryBase<CourseReview> CourseReviews { get; }
    ICourseExerciseRepository Puzzles { get; }

    Task<bool> SaveChangesAsync();
}