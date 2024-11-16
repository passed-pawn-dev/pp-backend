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
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TDto>> GetAllAsync<TDto>()
    {
        return await _dbSet
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TDto>> GetAllWhereAsync<TDto>(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(predicate)
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TDto?> GetByIdAsync<TDto>(int id)
    {
        return await _dbSet
            .Where(x => x.Id == id)
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public T Add(T entity)
    {
        _dbSet.Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }
}