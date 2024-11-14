using System.Linq.Expressions;
using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IRepositoryBase<T> where T : IEntity
{
    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>IEnumerable of entities.</returns>
    public Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Gets all entities and maps them to a DTO.
    /// </summary>
    /// <typeparam name="TDto">Type of a DTO.</typeparam>
    /// <returns>IEnumerable of DTOs.</returns>
    public Task<IEnumerable<TDto>> GetAllAsync<TDto>();

    /// <summary>
    /// Gets all entities that satisfy a predicate.
    /// </summary>
    /// <param name="predicate">Predicate.</param>
    /// <returns>IEnumerable of entities.</returns>
    public Task<IEnumerable<T>> GetAllWhereAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Gets all entities that satisfy a predicate and maps them to a DTO.
    /// </summary>
    /// <param name="predicate">Predicate</param>
    /// <typeparam name="TDto">Type of a DTO.</typeparam>
    /// <returns>IEnumerable of DTOs.</returns>
    public Task<IEnumerable<TDto>> GetAllWhereAsync<TDto>(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    /// <param name="id">Id of an entity.</param>
    /// <returns>Entity, or null.</returns>
    public Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Gets an entity by its ID and maps it to a DTO.
    /// </summary>
    /// <param name="id">Id of an entity.</param>
    /// <typeparam name="TDto">Type of a DTO.</typeparam>
    /// <returns>DTO, or null.</returns>
    public Task<TDto?> GetByIdAsync<TDto>(int id);

    /// <summary>
    /// Adds an entity. Changes need to be saved after.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <returns>Added entity.</returns>
    public T Add(T entity);

    /// <summary>
    /// Updates an entity. Changes need to be saved after.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <returns>Updated entity.</returns>
    public T Update(T entity);

    /// <summary>
    /// Deletes an entity. Changes need to be saved after.
    /// </summary>
    /// <param name="entity">Entity to delete.</param>
    public void Delete(T entity);
    
    public bool Exists(int id);
}