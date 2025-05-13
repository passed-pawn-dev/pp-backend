using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.DataAccess.Repositories.Contracts;

public interface ICourseVideoRepository : IRepositoryBase<CourseVideo>
{
    Task<CourseVideoDto?> GetOwnedOrInPreviewAsync(int videoId, int userId);
}