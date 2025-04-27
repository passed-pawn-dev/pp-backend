using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICoachRepository : IRepositoryBase<Coach>
{
    Task<int?> GetUserIdByEmail(string email);
    Task<Coach?> GetWithPhotoById(int id);
}
