using PassedPawn.DataAccess.Entities;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IStudentRepository : IRepositoryBase<Student>
{
    Task<int?> GetIdByEmail(string email);
    Task<Student?> GetUserByEmail(string email);
    Task<IEnumerable<BoughtCourseDto>> GetStudentCourses(int userId);
    Task<NonBoughtCourseDetailsDto?> GetStudentCourse(int userId, int courseId);
    Task<bool> IsCourseBought(int userId, int courseId);
}