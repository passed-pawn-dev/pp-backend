using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ILessonRepository : IRepositoryBase<Lesson>
{
    Task<IEnumerable<LessonDto>> GetUserLessons(int userId, int courseId);
}
