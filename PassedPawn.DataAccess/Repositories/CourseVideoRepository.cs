using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.DataAccess.Repositories;

public class CourseVideoRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<CourseVideo>(dbContext, mapper), ICourseVideoRepository
{
    public async Task<CourseVideoDto?> GetOwnedOrInPreviewAsync(int videoId, int userId)
    {
        return await DbSet
            .Include(video => video.Lesson)
            .ThenInclude(lesson => lesson!.Course)
            .ThenInclude(course => course!.Students)
            .Where(video => video.Id == videoId &&
                           (video.Lesson!.Preview || video.Lesson.Course!.Students.Any(student => student.Id == userId) || video.Lesson.Course!.Coach!.Id == userId))
            .ProjectTo<CourseVideoDto>(MapperConfiguration)
            .SingleOrDefaultAsync();
    }
}
