using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IMapper mapper) : IUnitOfWork
{
    public IRepositoryBase<Student> Students { get; } = new RepositoryBase<Student>(dbContext, mapper);
    public IRepositoryBase<Coach> Coaches { get; } = new RepositoryBase<Coach>(dbContext, mapper);
    public IRepositoryBase<Nationality> Nationalities { get; } = new RepositoryBase<Nationality>(dbContext, mapper);
    public IRepositoryBase<Course> Courses { get; } = new RepositoryBase<Course>(dbContext, mapper);
    public IRepositoryBase<Lesson> Lessons { get; } = new RepositoryBase<Lesson>(dbContext, mapper);


    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}