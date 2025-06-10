using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Example;

namespace PassedPawn.DataAccess.Repositories;

public class CourseExampleRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<CourseExample>(dbContext, mapper), ICourseExampleRepository
{
    public async Task<CourseExampleDto?> GetOwnedOrInPreviewAsync(int exampleId, int userId)
    {
        return await DbSet
            .Include(example => example.Lesson)
            .ThenInclude(lesson => lesson!.Course)
            .ThenInclude(course => course!.Students)
            .Where(example => example.Id == exampleId &&
                           (example.Lesson!.Preview || example.Lesson.Course!.Students.Any(student => student.Id == userId) || example.Lesson.Course!.Coach!.Id == userId))
            .ProjectTo<CourseExampleDto>(MapperConfiguration)
            .SingleOrDefaultAsync();
    }
}
