using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IUnitOfWork
{
    IStudentRepository Students { get; }
    ICoachRepository Coaches { get; }
    IRepositoryBase<Photo> Photos { get; }
    IRepositoryBase<Nationality> Nationalities { get; }
    ICourseRepository Courses { get; }
    ILessonRepository Lessons { get; }
    IRepositoryBase<CourseReview> CourseReviews { get; }
    ICourseExerciseRepository Puzzles { get; }
    IRepositoryBase<CourseExample> Examples { get; }
    IRepositoryBase<CourseQuiz> Quizzes { get; }
    IRepositoryBase<CourseVideo> Videos { get; }

    Task<bool> SaveChangesAsync();
}