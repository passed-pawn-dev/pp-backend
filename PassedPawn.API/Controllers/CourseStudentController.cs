using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Review;
using PassedPawn.Models.Params;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

[ApiController]
[Route("api/Course/Student")]
public class CourseStudentController(IUnitOfWork unitOfWork,
    IClaimsPrincipalService claimsPrincipalService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseDto>))]
    [SwaggerOperation(
        Summary = "Returns all courses' previews, to be displayed in a list"
    )]
    public async Task<IActionResult> GetAllCourses([FromQuery] GetAllCoursesQueryParams queryParams,
        IHttpService httpService)
    {
        var userId = await claimsPrincipalService.GetStudentIdOptional(User);

        var pagedList = await unitOfWork.Courses.GetAllWhereAsync(userId, queryParams);
        httpService.AddPaginationHeader(Response, PaginationHeader.FromPagedList(pagedList));
        return Ok(pagedList.Items);
    }

    [HttpGet("bought")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BoughtCourseDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Returns all courses owned by a student"
    )]
    public async Task<IActionResult> GetAllBoughtCourses([FromQuery] string? name)
    {
        var userId = await claimsPrincipalService.GetStudentId(User);
        return Ok(await unitOfWork.Students.GetStudentCoursesWhere(userId, name));
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NonBoughtCourseDetailsDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns course details. Should be used to display non bought course"
    )]
    public async Task<IActionResult> GetCourseDetails(int id)
    {
        var course = await unitOfWork.Courses.GetByIdAsync<NonBoughtCourseDetailsDto>(id);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpGet("{id:int}/bought")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoughtCourseDetailsDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns bought course details"
    )]
    public async Task<IActionResult> GetBoughtCourseDetails(int id)
    {
        var userId = await claimsPrincipalService.GetStudentId(User);
        var course = await unitOfWork.Students.GetStudentCourse(userId, id);
        return course is null ? NotFound() : Ok(course);
    }
    
    [HttpPost("{id:int}/buy")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(ClientIntent))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerOperation(
        Summary = "Attempts to but a course"
    )]
    public async Task<IActionResult> BuyCourse(int id, IStripeService stripeService)
    {
        var student = await claimsPrincipalService.GetStudent(User);
        var course = await unitOfWork.Courses.GetWithStudentsById(id);

        if (course is null)
            return NotFound();

        if (course.Students.Contains(student))
            return Conflict("Course is already bought");

        var clientSecret = await stripeService.CreateCourseIntentSecret(student.Id, course.Price, course.Id);
        return Ok(new ClientIntent(clientSecret));
    }
    
    [HttpPost("{id:int}/course-list")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a course to user's list"
    )]
    public async Task<IActionResult> AddToCourses(int id)
    {
        var student = await claimsPrincipalService.GetStudent(User);
        var course = await unitOfWork.Courses.GetWithStudentsById(id);

        if (course is null)
            return NotFound();

        if (course.Students.Contains(student))
            return Conflict("Course already on the list");
        
        course.Students.Add(student);
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:int}/course-list")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a course to user's list"
    )]
    public async Task<IActionResult> RemoveFromCourses(int id)
    {
        var student = await claimsPrincipalService.GetStudent(User);
        var course = await unitOfWork.Courses.GetWithStudentsById(id);

        if (course is null)
            return NotFound();

        if (!course.Students.Contains(student))
            return BadRequest("Course is not on the list");
        
        course.Students.Remove(student);
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{courseId:int}/lessons")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BoughtCourseDetailsLessonDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Gets course's lessons' previews")]
    public async Task<IActionResult> GetCourseLessons(int courseId, ILessonRepository lessonRepository)
    {
        var userId = await claimsPrincipalService.GetStudentId(User);
        var lessons = await lessonRepository.GetUserLessons(userId, courseId);
        return lessons.Any() ? Ok(lessons) : NotFound();
    }
    
    [HttpPost("{courseId:int}/review")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseReviewDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a new review to a course"
    )]
    public async Task<IActionResult> AddReview(int courseId, ICourseService courseService,
        CourseReviewUpsertDto reviewUpsertDto)
    {
        var course = await unitOfWork.Courses.GetByIdAsync(courseId);

        if (course is null)
            return NotFound();

        var userId = await claimsPrincipalService.GetStudentId(User);
        var courseReviewDto = await courseService.AddReview(userId, course, reviewUpsertDto);
        return CreatedAtAction("GetReview", "CourseReview", new { id = courseReviewDto.Id },
            courseReviewDto);
    }
}
