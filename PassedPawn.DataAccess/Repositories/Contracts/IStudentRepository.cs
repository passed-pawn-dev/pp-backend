using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IStudentRepository : IRepositoryBase<Student>
{
    Task<Student?> GetUserByEmail(string email);
}