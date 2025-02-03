using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class CourseExerciseRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<CourseExercise>(dbContext, mapper), ICourseExerciseRepository
{
    public async Task<CourseExercise?> GetPuzzleById(int id)
    {
        return await DbSet
            .Where(puzzle => puzzle.Id == id)
            .Include(puzzle => puzzle.Students)
            .SingleOrDefaultAsync();
    }
}
