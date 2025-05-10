using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<IEnumerable<CourseDto>> GetAllWhereAsync(int? userId, string? name, bool onlyBought);
    Task<Course?> GetByLessonId(int id);
    Task<Course?> GetWithLessonsById(int id);
    Task<Course?> GetWithStudentsById(int id);
    Task<Course?> GetWithThumbnailById(int id);
}