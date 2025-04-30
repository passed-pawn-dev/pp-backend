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
        var video = mapper.Map<CourseVideo>(addDto);
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (video.Order > highestOrderNumber + 1 || video.Order < 1)
            return ServiceResult<CourseVideoDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber + 1} expected"
            ]);

        var uploadResult = await cloudinaryService.UploadVideoAsync(addDto.Video);
        
        if (uploadResult.Error is not null)
            return ServiceResult<CourseVideoDto>.Failure([uploadResult.Error.Message]);

        try
        {
            video.VideoUrl = uploadResult.Url.AbsoluteUri;
            video.VideoPublicId = uploadResult.PublicId;

            MoveOrderOnAdd(lesson, video.Order);
            lesson.Videos.Add(video);
            unitOfWork.Lessons.Update(lesson);

            if (!await unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to save database");

            return ServiceResult<CourseVideoDto>.Success(mapper.Map<CourseVideoDto>(video));
        }
        catch
        {
            await cloudinaryService.DeleteVideoAsync(uploadResult.PublicId);
            throw;
        }
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

        if (updateDto.Video is not null)
        {
            var oldVideoUrl = video.VideoUrl;
            var oldVideoPublicId = video.VideoPublicId;
            
            var uploadResult = await cloudinaryService.UploadVideoAsync(updateDto.Video);
        
            if (uploadResult.Error is not null)
                return ServiceResult<CourseVideoDto>.Failure([uploadResult.Error.Message]);

            try
            {
                video.VideoUrl = uploadResult.Url.AbsoluteUri;
                video.VideoPublicId = uploadResult.PublicId;
                MoveOrderOnUpdate(lesson, video.Order, updateDto.Order);
                mapper.Map(updateDto, video);
                unitOfWork.Videos.Update(video);

                if (!await unitOfWork.SaveChangesAsync())
                    throw new Exception("Failed to save database");
                
                await cloudinaryService.DeleteVideoAsync(oldVideoPublicId);
            }
            catch
            {
                video.VideoUrl = oldVideoUrl;
                video.VideoPublicId = oldVideoPublicId;
                await cloudinaryService.DeleteVideoAsync(uploadResult.PublicId);
                throw;
            }
        }
        else
        {
            MoveOrderOnUpdate(lesson, video.Order, updateDto.Order);
            mapper.Map(updateDto, video);
            unitOfWork.Videos.Update(video);

            if (!await unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to save database");
        }

        return ServiceResult<CourseVideoDto>.Success(mapper.Map<CourseVideoDto>(video));
    }
}