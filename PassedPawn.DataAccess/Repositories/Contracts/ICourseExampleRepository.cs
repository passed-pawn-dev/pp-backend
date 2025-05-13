using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.DTOs.Course.Example;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseExampleRepository : IRepositoryBase<CourseExample>
{
    Task<CourseExampleDto?> GetOwnedOrInPreviewAsync(int exampleId, int userId);
}
