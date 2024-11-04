using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IMapper mapper) : IUnitOfWork
{
    public IRepositoryBase<Student> Students { get; } = new RepositoryBase<Student>(dbContext, mapper);
    
    public IRepositoryBase<Coach> Coaches { get; } = new RepositoryBase<Coach>(dbContext, mapper);

    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}