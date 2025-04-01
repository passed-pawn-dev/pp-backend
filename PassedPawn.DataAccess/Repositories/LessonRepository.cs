using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.DataAccess.Repositories;

public class LessonRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<Lesson>(dbContext, mapper), ILessonRepository
{
    public async Task<IEnumerable<LessonDto>> GetUserLessons(int userId, int courseId)
    {
        return await DbSet
            .Include(lesson => lesson.Course)
            .ThenInclude(course => course!.Students)
            .Where(lesson =>
                lesson.CourseId == courseId && lesson.Course!.Students.Any(student => student.Id == userId))
            .ProjectTo<LessonDto>(MapperConfiguration)
            .ToListAsync();
    }

    public async Task<Lesson?> GetWithElementsAndCoachById(int lessonId)
    {
        return await DbSet
            .Where(lesson => lesson.Id == lessonId)
            .Include(lesson => lesson.Examples)
            .Include(lesson => lesson.Exercises)
            .Include(lesson => lesson.Course)
            .SingleOrDefaultAsync();

    }

    public async Task<Lesson?> GetByExampleId(int exampleId)
    {
        return await DbSet
            .Include(lesson => lesson.Examples)
            .Where(lesson => lesson.Examples.Any(example => example.Id == exampleId))
            .Include(lesson => lesson.Exercises)
            .Include(lesson => lesson.Course)
            .SingleOrDefaultAsync();

    }
}
