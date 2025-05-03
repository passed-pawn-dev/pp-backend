using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class CoursePuzzleRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<CoursePuzzle>(dbContext, mapper), ICoursePuzzleRepository
{
    public async Task<CoursePuzzle?> GetPuzzleById(int id)
    {
        return await DbSet
            .Where(puzzle => puzzle.Id == id)
            .Include(puzzle => puzzle.Students)
            .SingleOrDefaultAsync();
    }
}
