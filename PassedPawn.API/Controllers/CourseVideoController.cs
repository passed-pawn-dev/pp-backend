using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs;
using PassedPawn.Models.DTOs.Course.Video;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseVideoController(IUnitOfWork unitOfWork, IClaimsPrincipalService claimsPrincipalService,
    ICourseVideoService videoService) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [Authorize(Policy = "require student or coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseVideoDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single video by id"
    )]
    public async Task<IActionResult> Get(int id, ICloudinaryService cloudinaryService)
    {
        var userId = await claimsPrincipalService.GetStudentId(User);
        var video = await unitOfWork.Videos.GetOwnedOrInPreviewAsync(id, userId);
        
        if (video == null)
        {
            return NotFound();
        }
        
        var temporaryVideoUrl = cloudinaryService.GetDownloadUrl(video.VideoPublicId, "video", "mp4", 120, true);
        video.TemporaryVideoDownloadUrl = temporaryVideoUrl;
        
        return Ok(video);
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseVideoDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Updates a video",
        Description = "New video's order can be in the middle of the lesson, so other elements' orders might be modified to account for that.\n" +
                      "All of the properties will be overriden, except for video. When left as null, old video will be kept"
    )]
    public async Task<IActionResult> UpdateVideo(int id, CourseVideoUpdateDto updateDto)
    {
        var lesson = await unitOfWork.Lessons.GetByVideoId(id);

        if (lesson is null)
            return NotFound();

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

        var serviceResult = await videoService.ValidateAndUpdateVideo(lesson, id, updateDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseVideoDto = serviceResult.Data;
        return Ok(courseVideoDto);
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a video"
    )]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByVideoId(id);

        if (lesson is null)
            return NotFound();
        
        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

        var courseVideo = lesson.Videos.Single(video => video.Id == id);
        await videoService.DeleteVideo(lesson, courseVideo);
        return NoContent();
    }

    [HttpGet("upload-signature")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CloudinarySecureUrl))]
    public IActionResult GetUploadSignature(ICloudinaryService cloudinaryService)
    {
        return Ok(cloudinaryService.GetUploadSignature("lesson_videos", "video", "private"));
    }
}
