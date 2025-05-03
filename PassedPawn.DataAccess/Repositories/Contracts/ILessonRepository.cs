using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ILessonRepository : IRepositoryBase<Lesson>
{
    Task<IEnumerable<LessonDto>> GetUserLessons(int userId, int courseId);
    Task<Lesson?> GetWithElementsAndCoachById(int lessonId);
    Task<Lesson?> GetByExampleId(int exampleId);
    Task<Lesson?> GetByPuzzleId(int puzzleId);
    Task<Lesson?> GetByVideoId(int videoId);
    Task<Lesson?> GetByQuizId(int quizId);
}
