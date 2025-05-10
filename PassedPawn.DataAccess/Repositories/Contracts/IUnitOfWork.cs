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
    ICoursePuzzleRepository Puzzles { get; }
    ICourseExampleRepository Examples { get; }
    ICourseQuizRepository Quizzes { get; }
    ICourseVideoRepository Videos { get; }

    Task<bool> SaveChangesAsync();
}