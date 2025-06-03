using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.BusinessLogic.Exceptions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

[ApiController]
[Route("api/Course/Coach")]
public class CourseCoachController(IUnitOfWork unitOfWork, ICourseService courseService,
    IClaimsPrincipalService claimsPrincipalService) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoachCourseDto>))]
    [SwaggerOperation(
        Summary = "Returns all coach courses' previews, to be displayed in a list"
    )]
    public async Task<IActionResult> GetAllCourses()
    {
        var coachId = await claimsPrincipalService.GetCoachId(User);
        return Ok(await unitOfWork.Coaches.GetCoachCourses(coachId));
    }
    
    [HttpGet("{id:int}/edit")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseEditViewDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Gets course for edit"
    )]
    public async Task<IActionResult> GetCourseEdit(int id)
    {
        var coachId = await claimsPrincipalService.GetCoachId(User);
        var courseDto = await unitOfWork.Coaches.GetCoachCourse<CourseEditViewDto>(id, coachId);
        return courseDto is null ? NotFound() : Ok(courseDto);
    }
    
    [HttpGet("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoachCourseDetailsDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Gets coach course details"
    )]
    public async Task<IActionResult> GetCourseDetails(int id)
    {
        var coachId = await claimsPrincipalService.GetCoachId(User);
        var courseDto = await unitOfWork.Coaches.GetCoachCourse<CoachCourseDetailsDto>(id, coachId);
        return courseDto is null ? NotFound() : Ok(courseDto);
    }
    
    [HttpPost]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [SwaggerOperation(
        Summary = "Creates a course"
    )]
    public async Task<IActionResult> CreateCourse(CourseUpsertDto courseUpsertDto)
    {
        var coachId = await claimsPrincipalService.GetCoachId(User);
        var serviceResult = await courseService.ValidateAndAddCourse(coachId, courseUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseDto = serviceResult.Data;
        return CreatedAtAction("GetCourseDetails", "CourseStudent", new { id = courseDto.Id }, courseDto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Updates a course"
    )]
    public async Task<IActionResult> UpdateCourse(int id, CourseUpsertDto courseUpsertDto)
    {
        var course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (course.CoachId != coachId)
            return Forbid();

        var serviceResult = await courseService.ValidateAndUpdateCourse(course, courseUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseDto = serviceResult.Data!;
        return Ok(courseDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a course"
    )]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (course.CoachId != coachId)
            return Forbid();

        unitOfWork.Courses.Delete(course);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }
    
    [HttpPost("{courseId:int}/lesson")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LessonDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a new lesson to a course",
        Description = "New lesson's order can be in the middle of the course, so other lessons' orders might be modified to account for that."
    )]
    public async Task<IActionResult> AddLesson(int courseId, LessonUpsertDto lessonUpsertDto)
    {
        var course = await unitOfWork.Courses.GetWithLessonsById(courseId);

        if (course is null)
            return NotFound();

        if (course.CoachId != await claimsPrincipalService.GetCoachId(User))
            return Forbid();

        var serviceResult = await courseService.ValidateAndAddLesson(course, lessonUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var lessonDto = serviceResult.Data;

        return CreatedAtAction("GetLesson", "Lesson", new { id = lessonDto.Id }, lessonDto);
    }
    
    [HttpPatch("{courseId:int}/thumbnail")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PhotoDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Adds or updates thumbnail")]
    public async Task<IActionResult> UploadThumbnail(int courseId, CourseThumbnailDto thumbnailDto,
        ICloudinaryService cloudinaryService)
    {
        var userId = await claimsPrincipalService.GetCoachId(User);
        var course = await unitOfWork.Courses.GetWithThumbnailById(courseId);

        if (course is null)
            return NotFound();

        if (course.CoachId != userId)
            return Forbid();

        if (!cloudinaryService.IsUrlValid(thumbnailDto.PhotoUrl))
            return BadRequest("Invalid photo url");

        if (course.Thumbnail is null)
        {
            course.Thumbnail = new Photo
            {
                Url = thumbnailDto.PhotoUrl,
                PublicId = thumbnailDto.PhotoPublicId
            };
        }
        else
        {
            _ = cloudinaryService.DeletePhotoAsync(course.Thumbnail.PublicId); // Fire and forget
            course.Thumbnail.Url = thumbnailDto.PhotoUrl;
            course.Thumbnail.PublicId = thumbnailDto.PhotoPublicId;
        }

        await unitOfWork.SaveChangesAsync();
        return Ok(new PhotoDto(thumbnailDto.PhotoUrl, thumbnailDto.PhotoPublicId));
    }
    
    [HttpGet("thumbnail/signature")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CloudinarySecureUrl))]
    public IActionResult GetUploadSignature(ICloudinaryService cloudinaryService)
    {
        return Ok(cloudinaryService.GetUploadSignature("course_thumbnail", "image", "upload"));
    }

    [HttpDelete("{courseId:int}/thumbnail")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Deletes a thumbnail")]
    public async Task<IActionResult> DeleteThumbnail(int courseId,
        ICloudinaryService cloudinaryService)
    {
        var userId = await claimsPrincipalService.GetCoachId(User);
        var course = await unitOfWork.Courses.GetWithThumbnailById(courseId);

        if (course is null)
            return NotFound();

        if (course.CoachId != userId)
            return Forbid();

        if (course.Thumbnail is null)
            return NotFound(new { message = "Course has no thumbnail" });

        _ = cloudinaryService.DeletePhotoAsync(course.Thumbnail.PublicId); // Fire and forget
        unitOfWork.Photos.Delete(course.Thumbnail);
        course.Thumbnail = null;
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{courseId:int}/lesson-count")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LessonCountDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets course's lesson count")]
    public async Task<IActionResult> GetLessonCount(int courseId)
    {
        var coachId = await claimsPrincipalService.GetCoachId(User);
        var lessonCount = await unitOfWork.Courses.GetLessonCount(coachId, courseId);
        return lessonCount is null ? NotFound() : Ok(new LessonCountDto(lessonCount.Value));
    }
    
}
