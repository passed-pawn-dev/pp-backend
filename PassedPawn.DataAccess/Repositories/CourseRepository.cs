using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories;

public class CourseRepository(ApplicationDbContext dbContext, IMapper mapper) :
    RepositoryBase<Course>(dbContext, mapper), ICourseRepository
{
    public async Task<IEnumerable<CourseDto>> GetAllAsync(int? userId)
    {
        return await DbSet
            .Include(c => c.Coach)
            .Include(c => c.Reviews)
            .Include(c => c.Thumbnail)
            .Include(c => c.Students)
            .Select(course => new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                EloRageStart = course.EloRangeStart,
                EloRangeEnd = course.EloRangeEnd,
                CoachName = $"{course.Coach!.FirstName} {course.Coach.LastName}",
                AverageScore = course.Reviews.Count > 0 ? course.Reviews.Average(review => review.Value) : 0,
                PictureUrl = course.Thumbnail == null ? null : course.Thumbnail.Url,
                IsBought = userId != null && course.Students.Any(student => student.Id == userId.Value)
            })
            .ToListAsync();
    }

    public async Task<Course?> GetByLessonId(int id)
    {
        return await DbSet
            .Include(course => course.Lessons)
            .ThenInclude(lesson => lesson.Puzzles)
            .SingleOrDefaultAsync(course => course.Lessons.Any(lesson => lesson.Id == id));
    }

    public async Task<Course?> GetWithLessonsById(int id)
    {
        return await DbSet
            .Include(course => course.Lessons)
            .ThenInclude(lesson => lesson.Puzzles)
            .SingleOrDefaultAsync(course => course.Id == id);
    }

    public async Task<Course?> GetWithStudentsById(int id)
    {
        return await DbSet
            .Include(course => course.Students)
            .SingleOrDefaultAsync(course => course.Id == id);
    }

    public async Task<Course?> GetWithThumbnailById(int id)
    {
        return await DbSet
            .Where(course => course.Id == id)
            .Include(course => course.Thumbnail)
            .SingleOrDefaultAsync();
    }
}