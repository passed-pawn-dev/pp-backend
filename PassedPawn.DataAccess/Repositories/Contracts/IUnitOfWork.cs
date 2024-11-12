using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IUnitOfWork
{
    // definiowanie repozytoriów
    IRepositoryBase<Student> Students { get; }
    IRepositoryBase<Coach> Coaches { get; }
    IRepositoryBase<Nationality> Nationalities { get; }
    IRepositoryBase<Course> Courses { get; }
    IRepositoryBase<Lesson> Lessons { get; }
    
    Task<bool> SaveChangesAsync();
}