using AutoMapper;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.BusinessLogic.Services;

public class CourseVideoService(IUnitOfWork unitOfWork, IMapper mapper,
    ICloudinaryService cloudinaryService) : CourseElementService, ICourseVideoService
{
    public async Task<ServiceResult<CourseVideoDto>> ValidateAndAddVideo(Lesson lesson,
        CourseVideoAddDto addDto)
    {
        var highestOrderNumber = GetHighestOrderNumber(lesson) + 1;
        addDto.Order ??= highestOrderNumber;
        var video = mapper.Map<CourseVideo>(addDto);

        if (video.Order > highestOrderNumber || video.Order < 1)
            return ServiceResult<CourseVideoDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber} expected"
            ]);
        
        MoveOrderOnAdd(lesson, video.Order);
        lesson.Videos.Add(video);
        unitOfWork.Lessons.Update(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseVideoDto>.Success(mapper.Map<CourseVideoDto>(video));
    }

    public async Task<ServiceResult<CourseVideoDto>> ValidateAndUpdateVideo(Lesson lesson, int exampleId,
        CourseVideoUpdateDto updateDto)
    {
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (updateDto.Order > highestOrderNumber || updateDto.Order < 1)
            return ServiceResult<CourseVideoDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        var video = lesson.Videos.Single(video => video.Id == exampleId);
        
        MoveOrderOnUpdate(lesson, video.Order, updateDto.Order);
        mapper.Map(updateDto, video);

        unitOfWork.Videos.Update(video);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseVideoDto>.Success(mapper.Map<CourseVideoDto>(video));
    }

    public async Task DeleteVideo(Lesson lesson, CourseVideo courseVideo)
    {
        var publicId = courseVideo.VideoPublicId;
        unitOfWork.Videos.Delete(courseVideo);
        MoveOrderOnDelete(lesson, courseVideo.Order);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");
        
        _ = cloudinaryService.DeleteVideoAsync(publicId); // Fire and forget
    }
}
