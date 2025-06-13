using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.DTOs.Course.Quiz;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseQuizRepository : IRepositoryBase<CourseQuiz>
{
    Task<CourseQuizDto?> GetOwnedOrInPreviewForStudentAsync(int quizId, int userId);
    Task<CourseQuizDto?> GetOwnedOrInPreviewForCoachAsync(int quizId, int userId);
}
