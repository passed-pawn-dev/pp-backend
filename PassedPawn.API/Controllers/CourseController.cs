using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseController(IUnitOfWork unitOfWork, ICourseService courseService,
    IClaimsPrincipalService claimsPrincipalService) : ApiControllerBase
{

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
