namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync();
}