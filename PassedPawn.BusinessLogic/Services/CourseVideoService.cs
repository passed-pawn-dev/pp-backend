using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.BusinessLogic.Services;

public class CourseVideoService(IUnitOfWork unitOfWork, IMapper mapper,
    Cloudinary cloudinary) : CourseElementService, ICourseVideoService
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
        
        await using var stream = upsertDto.Video.OpenReadStream();
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(upsertDto.Video.FileName, stream),
            Folder = "lesson_videos"
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);
        video.VideoUrl = uploadResult.Url.AbsoluteUri;
        video.VideoPublicId = uploadResult.PublicId;

        MoveOrderOnAdd(lesson, video.Order);
        lesson.Videos.Add(video);
        unitOfWork.Lessons.Update(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseVideoDto>.Success(mapper.Map<CourseVideoDto>(video));
    }

    public Task<ServiceResult<CourseVideoDto>> ValidateAndUpdateVideo(Lesson lesson, int exampleId, CourseVideoUpsertDto upsertDto)
    {
        throw new NotImplementedException();
    }
}