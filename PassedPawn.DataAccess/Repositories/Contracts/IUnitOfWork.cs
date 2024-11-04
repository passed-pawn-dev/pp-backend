using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IUnitOfWork
{
    // definiowanie repozytoriów
    IRepositoryBase<Student> Students { get; }
    
    IRepositoryBase<Coach> Coaches { get; }
    
    Task<bool> SaveChangesAsync();
}