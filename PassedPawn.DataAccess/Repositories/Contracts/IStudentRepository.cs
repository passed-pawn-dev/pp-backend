using System.Linq.Expressions;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface IStudentRepository : IRepositoryBase<Student>
{
    Task<int?> GetIdByEmail(string email);
    Task<Student?> GetUserByEmail(string email);
    Task<IEnumerable<BoughtCourseDto>> GetStudentCoursesWhere(int userId,
        Expression<Func<Course, bool>>? predicate = null);
    Task<BoughtCourseDetailsDto?> GetStudentCourse(int userId, int courseId);
    Task<bool> IsCourseBought(int userId, int courseId);
}