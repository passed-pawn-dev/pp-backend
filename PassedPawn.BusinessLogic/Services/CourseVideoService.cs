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
        CourseVideoUpsertDto upsertDto)
    {
        var video = mapper.Map<CourseVideo>(upsertDto);
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (video.Order > highestOrderNumber + 1 || video.Order < 1)
            return ServiceResult<CourseVideoDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber + 1} expected"
            ]);

        var uploadResult = await cloudinaryService.UploadAsync(upsertDto.Video);
        video.VideoUrl = uploadResult.Url.AbsoluteUri;
        video.VideoPublicId = uploadResult.PublicId;

        MoveOrderOnAdd(lesson, video.Order);

        try
        {
            lesson.Videos.Add(video);
            unitOfWork.Lessons.Update(lesson);

            if (!await unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to save database");

            return ServiceResult<CourseVideoDto>.Success(mapper.Map<CourseVideoDto>(video));
        }
        catch
        {
            await cloudinaryService.DeleteAsync(uploadResult.PublicId);
            return ServiceResult<CourseVideoDto>.Failure([
                "Something went wrong"
            ]);
        }
    }

    public Task<ServiceResult<CourseVideoDto>> ValidateAndUpdateVideo(Lesson lesson, int exampleId, CourseVideoUpsertDto upsertDto)
    {
        throw new NotImplementedException();
    }
}