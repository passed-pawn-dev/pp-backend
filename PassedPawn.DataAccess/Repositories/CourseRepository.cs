using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class CourseRepository(ApplicationDbContext dbContext, IMapper mapper) :
    RepositoryBase<Course>(dbContext, mapper), ICourseRepository
{
    public async Task<Course?> GetByLessonId(int id)
    {
        return await DbSet
            .Include(course => course.Lessons)
            .ThenInclude(lesson => lesson.Exercises)
            .SingleOrDefaultAsync(course => course.Lessons.Any(lesson => lesson.Id == id));
    }

    public async Task<Course?> GetWithLessonsById(int id)
    {
        return await DbSet
            .Include(course => course.Lessons)
            .ThenInclude(lesson => lesson.Exercises)
            .SingleOrDefaultAsync(course => course.Id == id);
    }
}