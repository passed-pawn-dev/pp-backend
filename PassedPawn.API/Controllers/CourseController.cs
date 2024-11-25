using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;

namespace PassedPawn.API.Controllers;

public class CourseController(IUnitOfWork unitOfWork, ICourseService courseService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCourses() // TODO: Add filters
    {
        IEnumerable<CourseDto> courses = await unitOfWork.Courses.GetAllAsync<CourseDto>();
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await unitOfWork.Courses.GetByIdAsync<CourseDto>(id);

        if (course is null)
            return NotFound();

        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CourseUpsertDto courseUpsertDto)
    {
        ServiceResult<CourseDto> serviceResult = await courseService.ValidateAndAddCourse(courseUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        CourseDto? courseDto = serviceResult.Data;
        return CreatedAtAction(nameof(GetCourse), new { id = courseDto.Id }, courseDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCourse(int id, CourseUpsertDto courseUpsertDto)
    {
        Course? course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();

        ServiceResult<CourseDto> serviceResult = await courseService.ValidateAndUpdateCourse(course, courseUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        CourseDto courseDto = serviceResult.Data!;
        return Ok(courseDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        Course? course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();

        unitOfWork.Courses.Delete(course);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }

    #region Lessons

    [HttpGet("{id:int}/lesson")]
    public async Task<IActionResult> GetLessons(int id)
    {
        IEnumerable<LessonDto> lessons = await unitOfWork.Lessons
            .GetAllWhereAsync<LessonDto>(lesson => lesson.CourseId == id);

        return Ok(lessons);
    }

    [HttpPost("{id:int}/lesson")]
    public async Task<IActionResult> AddLesson(int id, LessonUpsertDto lessonUpsertDto)
    {
        Course? course = await unitOfWork.Courses.GetWithLessonsById(id);

        if (course is null)
            return NotFound();

        ServiceResult<LessonDto> serviceResult = await courseService.ValidateAndAddLesson(course, lessonUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        LessonDto? lessonDto = serviceResult.Data;

        return CreatedAtAction("GetLesson", "Lesson", new { id = lessonDto.Id }, lessonDto);
    }

    #endregion

    #region Reviews

    [HttpGet("{id:int}/review")]
    public async Task<IActionResult> GetReviews(int id)
    {
        IEnumerable<CourseReviewDto> reviews = await unitOfWork.CourseReviews
            .GetAllWhereAsync<CourseReviewDto>(review => review.CourseId == id);

        return Ok(reviews);
    }
    
    // TODO Add User Id, who creates this review
    // TODO Protect this endpoint

    [HttpPost("{id:int}/review")]
    public async Task<IActionResult> AddReview(int id, CourseReviewUpsertDto reviewUpsertDto)
    {
        Course? course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();

        CourseReviewDto courseReviewDto = await courseService.AddReview(course, reviewUpsertDto);
        return CreatedAtAction("GetReview", "CourseReview", new { id = courseReviewDto.Id },
            courseReviewDto);
    }

    #endregion
}