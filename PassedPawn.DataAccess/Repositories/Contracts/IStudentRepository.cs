using PassedPawn.DataAccess.Entities;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IStudentRepository : IRepositoryBase<Student>
{
    Task<int?> GetIdByEmail(string email);
    Task<Student?> GetUserByEmail(string email);
    Task<IEnumerable<BoughtCourseDto>> GetStudentCoursesWhere(int userId, string? name);
    Task<BoughtCourseDetailsDto?> GetStudentCourse(int userId, int courseId);
    Task<bool> IsCourseBought(int userId, int courseId);
}