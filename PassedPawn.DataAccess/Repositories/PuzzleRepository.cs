using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class PuzzleRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<Puzzle>(dbContext, mapper), IPuzzleRepository
{
    public async Task<Puzzle?> GetPuzzleById(int id)
    {
        return await DbSet
            .SingleOrDefaultAsync(puzzle => puzzle.Id == id);
    }
}
