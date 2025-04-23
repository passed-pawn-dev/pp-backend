using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

// TODO: REFACTOR: Rethink endpoints, DTOs, required roles
public class CourseController(IUnitOfWork unitOfWork, ICourseService courseService,
    IClaimsPrincipalService claimsPrincipalService) : ApiControllerBase
{

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
        return Ok(courseDto); // TODO: Should be 201. Should be changed during coach course refactor
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

    #region Lessons

    [HttpGet("{courseId:int}/lesson")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LessonDto>))]
    [SwaggerOperation(
        Summary = "Returns all lessons that belong to a course"
    )]
    public async Task<IActionResult> GetLessons(int courseId)
    {
        var userId = await claimsPrincipalService.GetStudentId(User);
        
        var lessons = await unitOfWork.Lessons
            .GetUserLessons(userId, courseId);

        return Ok(lessons);
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

    #endregion

    #region Reviews

    [HttpGet("{courseId:int}/review")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseReviewDto>))]
    [SwaggerOperation(
        Summary = "Returns all reviews that belong to a course"
    )]
    public async Task<IActionResult> GetReviews(int courseId)
    {
        var reviews = await unitOfWork.CourseReviews
            .GetAllWhereAsync<CourseReviewDto>(review => review.CourseId == courseId);

        return Ok(reviews);
    }
    
    [HttpPost("{courseId:int}/review")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseReviewDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a new review to a course"
    )]
    public async Task<IActionResult> AddReview(int courseId, CourseReviewUpsertDto reviewUpsertDto)
    {
        var course = await unitOfWork.Courses.GetByIdAsync(courseId);

        if (course is null)
            return NotFound();

        var userId = await claimsPrincipalService.GetStudentId(User);
        var courseReviewDto = await courseService.AddReview(userId, course, reviewUpsertDto);
        return CreatedAtAction("GetReview", "CourseReview", new { id = courseReviewDto.Id },
            courseReviewDto);
    }

    #endregion
}
