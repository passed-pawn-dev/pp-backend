using PassedPawn.DataAccess.Entities.Courses.Elements;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseExampleMoveRepository: IRepositoryBase<CourseExampleMove>
{
    Task<int> DeleteAllForExampleAsync(int exampleId);
}