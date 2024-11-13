using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IUnitOfWork
{
    IRepositoryBase<Student> Students { get; }
    IRepositoryBase<Coach> Coaches { get; }
    IRepositoryBase<Nationality> Nationalities { get; }
    ICourseRepository Courses { get; }
    IRepositoryBase<Lesson> Lessons { get; }
    IRepositoryBase<CourseReview> CourseReviews { get; }
    
    Task<bool> SaveChangesAsync();
}
