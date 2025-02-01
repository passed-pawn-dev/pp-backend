using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IStudentRepository : IRepositoryBase<Student>
{
    Task<int?> GetUserIdByEmail(string email);
}