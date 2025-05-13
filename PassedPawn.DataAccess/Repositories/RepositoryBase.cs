using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class RepositoryBase<T>(
    ApplicationDbContext dbContext,
    IMapper mapper) : IRepositoryBase<T> where T : class, IEntity
{
    protected readonly ApplicationDbContext DbContext = dbContext;
    protected readonly DbSet<T> DbSet = dbContext.Set<T>();
    protected readonly IConfigurationProvider MapperConfiguration = mapper.ConfigurationProvider;

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<TDto>> GetAllAsync<TDto>()
    {
        return await DbSet
            .ProjectTo<TDto>(MapperConfiguration)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TDto>> GetAllWhereAsync<TDto>(Expression<Func<T, bool>> predicate)
    {
        return await DbSet
            .Where(predicate)
            .ProjectTo<TDto>(MapperConfiguration)
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<TDto?> GetByIdAsync<TDto>(int id)
    {
        return await DbSet
            .Where(x => x.Id == id)
            .ProjectTo<TDto>(MapperConfiguration)
            .FirstOrDefaultAsync();
    }

    public T Add(T entity)
    {
        DbSet.Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        DbSet.Update(entity);
        return entity;
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await DbSet.AnyAsync(e => e.Id == id);
    }
}