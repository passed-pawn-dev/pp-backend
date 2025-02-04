using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.API.Extensions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseController(IUnitOfWork unitOfWork, ICourseService courseService) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseDto>))]
    [SwaggerOperation(
        Summary = "Returns all courses"
    )]
    public async Task<IActionResult> GetAllCourses([FromQuery] bool paid)
    {
        if (paid)
        {
            var email = User.GetUserEmailOptional();

            if (email is null)
                return Unauthorized();

            var userId = await unitOfWork.Students.GetIdByEmail(email) 
                         ?? throw new Exception("User does not exist in database");

            var userCourses = await unitOfWork.Students.GetStudentCourses(userId);
            return Ok(userCourses);
        }
        
        var courses = await unitOfWork.Courses.GetAllAsync<CourseDto>();
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single course by id"
    )]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await unitOfWork.Courses.GetByIdAsync<NonUserCourse>(id);

        if (course is null)
            return NotFound();

        return Ok(course);
    }
    
    [HttpGet("{id:int}/bought")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single course by id, when bought by user"
    )]
    public async Task<IActionResult> GetCourseBought(int id)
    {
        var userEmail = User.GetUserEmail();

        var userId = await unitOfWork.Students.GetIdByEmail(userEmail)
                     ?? throw new Exception("User does not exist in database");
        
        var course = await unitOfWork.Courses.GetByIdAsync<CourseDetails>(id);

        if (course is null)
            return NotFound();
        
        if (!await unitOfWork.Students.IsCourseBought(userId, id))
            return Forbid();

        return Ok(course);
    }

    [HttpPost]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [SwaggerOperation(
        Summary = "Creates a course"
    )]
    public async Task<IActionResult> CreateCourse(CourseUpsertDto courseUpsertDto)
    {
        var coachId = await unitOfWork.Coaches.GetUserIdByEmail(User.GetUserEmail())
                      ?? throw new Exception("User does not exist in database");
        
        var serviceResult = await courseService.ValidateAndAddCourse(coachId, courseUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseDto = serviceResult.Data;
        return CreatedAtAction(nameof(GetCourse), new { id = courseDto.Id }, courseDto);
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
        
        var coachId = await unitOfWork.Coaches.GetUserIdByEmail(User.GetUserEmail())
                      ?? throw new Exception("User does not exist in database");

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
        
        var coachId = await unitOfWork.Coaches.GetUserIdByEmail(User.GetUserEmail())
                      ?? throw new Exception("User does not exist in database");

        if (course.CoachId != coachId)
            return Forbid();

        unitOfWork.Courses.Delete(course);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }

    [HttpPost("{id:int}/course-list")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a course to user's list"
    )]
    public async Task<IActionResult> AddToCourses(int id)
    {
        var userEmail = User.GetUserEmail();
        var student = await unitOfWork.Students.GetUserByEmail(userEmail);

        if (student is null)
            throw new Exception("User does not exist in database");

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
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a course to user's list"
    )]
    public async Task<IActionResult> RemoveFromCourses(int id)
    {
        var userEmail = User.GetUserEmail();
        var student = await unitOfWork.Students.GetUserByEmail(userEmail);

        if (student is null)
            throw new Exception("User does not exist in database");

        var course = await unitOfWork.Courses.GetWithStudentsById(id);

        if (course is null)
            return NotFound();

        if (!course.Students.Contains(student))
            return BadRequest("Course is not on the list");
        
        course.Students.Remove(student);
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    #region Lessons

    [HttpGet("{courseId:int}/lesson")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LessonDto>))]
    [SwaggerOperation(
        Summary = "Returns all lessons that belong to a course"
    )]
    public async Task<IActionResult> GetLessons(int courseId)
    {
        var lessons = await unitOfWork.Lessons
            .GetAllWhereAsync<LessonDto>(lesson => lesson.CourseId == courseId);

        return Ok(lessons);
    }

    [HttpPost("{courseId:int}/lesson")]
    [Authorize]
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
        
        var coachId = await unitOfWork.Coaches.GetUserIdByEmail(User.GetUserEmail())
                      ?? throw new Exception("User does not exist in database");

        if (course.CoachId != coachId)
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

    // TODO Add User Id, who creates this review
    // TODO Protect this endpoint

    [HttpPost("{courseId:int}/review")]
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

        var courseReviewDto = await courseService.AddReview(course, reviewUpsertDto);
        return CreatedAtAction("GetReview", "CourseReview", new { id = courseReviewDto.Id },
            courseReviewDto);
    }

    #endregion
}
