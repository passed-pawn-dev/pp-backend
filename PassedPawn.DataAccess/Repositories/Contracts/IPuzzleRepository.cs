using PassedPawn.DataAccess.Entities;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IPuzzleRepository : IRepositoryBase<Puzzle>
{
    Task<Puzzle?> GetPuzzleById(int id);
}
