using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseVideoRepository : IRepositoryBase<CourseVideo>
{
    Task<CourseVideoDto?> GetOwnedOrInPreviewForStudentAsync(int videoId, int userId);
    Task<CourseVideoDto?> GetOwnedOrInPreviewForCoachAsync(int videoId, int userId);

}