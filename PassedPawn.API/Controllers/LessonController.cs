using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.API.Extensions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Exercise;
using PassedPawn.Models.DTOs.Course.Lesson;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class LessonController(IUnitOfWork unitOfWork, ICourseService courseService) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LessonDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single lesson by id"
    )]
    public async Task<IActionResult> GetLesson(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByIdAsync<LessonDto>(id);

        if (lesson is null)
            return NotFound();

        return Ok(lesson);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LessonDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Updates a lesson",
        Description = "New lesson's order can be in the middle of the course, so other lessons' orders might be modified to account for that."
    )]
    public async Task<IActionResult> UpdateLesson(int id, LessonUpsertDto lessonUpsertDto)
    {
        var course = await unitOfWork.Courses.GetByLessonId(id);

        if (course is null)
            return NotFound();
        
        var coachId = await unitOfWork.Coaches.GetUserIdByEmail(User.GetUserEmail())
                      ?? throw new Exception("User does not exist in database");

        if (course.CoachId != coachId)
            return Forbid();

        var serviceResult =
            await courseService.ValidateAndUpdateLesson(course, id, lessonUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var lessonDto = serviceResult.Data;
        return Ok(lessonDto);
    }

    // TODO: Protect
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a lesson"
    )]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByIdAsync(id);

        if (lesson is null)
            return NotFound();

        unitOfWork.Lessons.Delete(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }
    
    // Only for coach
    //TODO roles
    [Authorize(Policy = "require coach role")]
    [HttpPost("{lessonId:int}/exercise")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseExerciseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Create new puzzle by Coach"
    )]
    public async Task<IActionResult> Post(int lessonId, CourseExerciseUpsertDto dto, IMapper mapper)
    {
        var email = User.GetUserEmail();
        var course = await unitOfWork.Courses.GetByLessonId(lessonId);

        if (course is null)
            return UnprocessableEntity("Invalid course");
        
        var courseId = await unitOfWork.Coaches.GetUserIdByEmail(email)
                       ?? throw new Exception("Coach exists in Keyclock but not in out database");

        if (course.CoachId != courseId)
            return Forbid();

        var exercise = mapper.Map<CourseExercise>(dto);
        var lesson = course.Lessons.First(lesson => lesson.Id == lessonId);
        lesson.Exercises.Add(exercise);
        await unitOfWork.SaveChangesAsync();
        var responseDto = mapper.Map<CourseExerciseDto>(exercise);
        return CreatedAtAction("Get", "CourseExercise", new { id = exercise.Id }, responseDto);
    }
}
