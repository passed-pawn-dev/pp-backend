using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class CourseExampleMoveRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<CourseExampleMove>(dbContext, mapper), ICourseExampleMoveRepository
{
    public async Task<int> DeleteAllForExampleAsync(int exampleId)
    {
        return await DbSet
            .Where(m => m.ExampleId == exampleId)
            .ExecuteDeleteAsync();
    }
}