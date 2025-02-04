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
}