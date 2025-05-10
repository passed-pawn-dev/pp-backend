using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Extensions;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.Params;

namespace PassedPawn.DataAccess.Repositories;

public class CourseRepository(ApplicationDbContext dbContext, IMapper mapper) :
    RepositoryBase<Course>(dbContext, mapper), ICourseRepository
{
    public async Task<PagedList<CourseDto>> GetAllWhereAsync(int? userId, GetAllCoursesQueryParams queryParams)
    {
        var query = DbSet
            .Include(c => c.Coach)
            .Include(c => c.Reviews)
            .Include(c => c.Thumbnail)
            .Include(c => c.Students)
            .AsQueryable();

        if (queryParams.EloRangeStart is not null)
            query = query.Where(course => course.EloRangeEnd == null || course.EloRangeEnd >= queryParams.EloRangeStart);

        if (queryParams.EloRangeEnd is not null)
            query = query.Where(course => course.EloRangeStart == null || course.EloRangeStart <= queryParams.EloRangeEnd);

        if (queryParams.Name is not null)
            query = query.Where(course => course.Title.ToLower().Contains(queryParams.Name.ToLower()));

        if (queryParams.MinPrice is not null)
            query = query.Where(course => course.Price >= queryParams.MinPrice);

        if (queryParams.MaxPrice is not null)
            query = query.Where(course => course.Price <= queryParams.MaxPrice);

        var selectQuery = query.Select(course => new CourseDto
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
            IsBought = userId != null && course.Students.Any(student => student.Id == userId.Value),
            EnrolledStudentsCount = course.Students.Count
        });
        
        if (queryParams.OnlyBought)
            selectQuery = selectQuery.Where(course => course.IsBought);

        selectQuery = queryParams.SortBy switch
        {
            GetAllCoursesSortOrder.Price => queryParams.SortDesc
                ? selectQuery.OrderByDescending(c => c.Price)
                : selectQuery.OrderBy(c => c.Price),
            GetAllCoursesSortOrder.AverageScore => queryParams.SortDesc
                ? selectQuery.OrderByDescending(c => c.AverageScore)
                : selectQuery.OrderBy(c => c.AverageScore),
            GetAllCoursesSortOrder.Popularity => queryParams.SortDesc
                ? selectQuery.OrderByDescending(c => c.EnrolledStudentsCount)
                : selectQuery.OrderBy(c => c.EnrolledStudentsCount),
            _ => selectQuery
        };

        return await selectQuery.ToPagedListAsync(queryParams.PageNumber, queryParams.PageSize);
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