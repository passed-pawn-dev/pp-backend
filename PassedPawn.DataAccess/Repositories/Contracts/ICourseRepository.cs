using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<Course?> GetByLessonId(int id);
    Task<Course?> GetWithLessonsById(int id);
    Task<Course?> GetWithStudentsById(int id);
    public Task<Course?> GetWithThumbnailById(int id);
}