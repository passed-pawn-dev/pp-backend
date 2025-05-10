using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Puzzle;

namespace PassedPawn.DataAccess.Repositories;

public class CoursePuzzleRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<CoursePuzzle>(dbContext, mapper), ICoursePuzzleRepository
{
    public async Task<CoursePuzzlesDto?> GetOwnedOrInPreviewAsync(int puzzleId, int userId)
    {
        return await DbSet
            .Include(puzzle => puzzle.Lesson)
            .ThenInclude(lesson => lesson!.Course)
            .ThenInclude(course => course!.Students)
            .Where(puzzle => puzzle.Id == puzzleId &&
                (puzzle.Lesson!.Preview || puzzle.Lesson.Course!.Students.Any(student => student.Id == userId)))
            .ProjectTo<CoursePuzzlesDto>(MapperConfiguration)
            .SingleOrDefaultAsync();
    }

    public async Task<CoursePuzzle?> GetPuzzleById(int id)
    {
        return await DbSet
            .Where(puzzle => puzzle.Id == id)
            .Include(puzzle => puzzle.Students)
            .SingleOrDefaultAsync();
    }
}
