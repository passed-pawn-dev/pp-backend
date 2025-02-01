using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IMapper mapper) : IUnitOfWork
{
    public ICoachRepository Coaches { get; } = new CoachRepository(dbContext, mapper);
    public IStudentRepository Students { get; } = new StudentRepository(dbContext, mapper);
    public IRepositoryBase<Nationality> Nationalities { get; } = new RepositoryBase<Nationality>(dbContext, mapper);
    public ICourseRepository Courses { get; } = new CourseRepository(dbContext, mapper);
    public IRepositoryBase<Lesson> Lessons { get; } = new RepositoryBase<Lesson>(dbContext, mapper);
    public IRepositoryBase<CourseReview> CourseReviews { get; } = new RepositoryBase<CourseReview>(dbContext, mapper);
    public IPuzzleRepository Puzzles { get; } = new PuzzleRepository(dbContext, mapper);


    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}