using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICourseVideoService
{
    public Task<ServiceResult<CourseVideoDto>> ValidateAndAddVideo(Lesson lesson, CourseVideoAddDto addDto);

    public Task<ServiceResult<CourseVideoDto>> ValidateAndUpdateVideo(Lesson lesson, int exampleId,
        CourseVideoUpdateDto updateDto);
    
    public Task DeleteVideo(Lesson lesson, CourseVideo courseVideo);
}
