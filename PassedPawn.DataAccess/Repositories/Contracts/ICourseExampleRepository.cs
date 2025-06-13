using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.DTOs.Course.Example;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseExampleRepository : IRepositoryBase<CourseExample>
{
    // separate for access control purposes 
    Task<CourseExampleDto?> GetOwnedOrInPreviewForStudentAsync(int exampleId, int userId);
    Task<CourseExampleDto?> GetOwnedOrInPreviewForCoachAsync(int exampleId, int userId);
}
