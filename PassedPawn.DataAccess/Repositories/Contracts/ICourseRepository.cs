using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.Params;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<IEnumerable<CourseDto>> GetAllWhereAsync(int? userId, GetAllCoursesQueryParams queryParams);
    Task<Course?> GetByLessonId(int id);
    Task<Course?> GetWithLessonsById(int id);
    Task<Course?> GetWithStudentsById(int id);
    Task<Course?> GetWithThumbnailById(int id);
}