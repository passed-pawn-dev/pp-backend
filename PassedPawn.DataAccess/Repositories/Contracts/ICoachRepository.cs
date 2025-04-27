using PassedPawn.DataAccess.Entities;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICoachRepository : IRepositoryBase<Coach>
{
    Task<IEnumerable<CoachCourseDto>> GetCoachCourses(int coachId);
    Task<T?> GetCoachCourse<T>(int courseId, int coachId);
    Task<int?> GetUserIdByEmail(string email);
}