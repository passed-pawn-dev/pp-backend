using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Exercise;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Video;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class LessonController(IUnitOfWork unitOfWork, ICourseService courseService,
    IClaimsPrincipalService claimsPrincipalService) : ApiControllerBase
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

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (course.CoachId != coachId)
            return Forbid();

        var serviceResult =
            await courseService.ValidateAndUpdateLesson(course, id, lessonUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var lessonDto = serviceResult.Data;
        return Ok(lessonDto);
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a lesson"
    )]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var course = await unitOfWork.Courses.GetByLessonId(id);

        if (course is null)
            return NotFound();

        var userId = await claimsPrincipalService.GetCoachId(User);

        if (course.CoachId != userId)
            return Forbid();

        var lesson = course.Lessons.First(lesson => lesson.Id == id);
        unitOfWork.Lessons.Delete(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }

    #region Elements

    [HttpPost("{lessonId:int}/example")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseExampleDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Adds a new example to a lesson",
        Description = "New example's order can be in the middle of the lesson, so other elements' orders might be modified to account for that."
    )]
    public async Task<IActionResult> AddExample(int lessonId, CourseExampleUpsertDto upsertDto,
        ICourseExampleService exampleService)
    {
        var lesson = await unitOfWork.Lessons.GetWithElementsAndCoachById(lessonId);

        if (lesson is null)
            return NotFound();

        if (lesson.Course?.CoachId != await claimsPrincipalService.GetCoachId(User))
            return Forbid();

        var serviceResult = await exampleService.ValidateAndAddExample(lesson, upsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseExampleDto = serviceResult.Data;
        return CreatedAtAction("Get", "CourseExample", new { id = courseExampleDto.Id }, courseExampleDto);
    }
    
    [Authorize(Policy = "require coach role")]
    [HttpPost("{lessonId:int}/exercise")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseExerciseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Adds a new exercise to a lesson",
        Description = "New exercise's order can be in the middle of the lesson, so other elements' orders might be modified to account for that."
    )]
    public async Task<IActionResult> Post(int lessonId, CourseExerciseUpsertDto upsertDto,
        ICourseExerciseService exerciseService)
    {
        var lesson = await unitOfWork.Lessons.GetWithElementsAndCoachById(lessonId);

        if (lesson is null)
            return NotFound();

        if (lesson.Course?.CoachId != await claimsPrincipalService.GetCoachId(User))
            return Forbid();

        var serviceResult = await exerciseService.ValidateAndAddExercise(lesson, upsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseExerciseDto = serviceResult.Data;
        return CreatedAtAction("Get", "CourseExercise", new { id = courseExerciseDto.Id }, courseExerciseDto);
    }
    
    [Authorize(Policy = "require coach role")]
    [HttpPost("{lessonId:int}/video")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseExerciseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Adds a new video to a lesson",
        Description = "New video's order can be in the middle of the lesson, so other elements' orders might be modified to account for that."
    )]
    public async Task<IActionResult> AddVideo(int lessonId, [FromForm] CourseVideoAddDto addDto,
        ICourseVideoService videoService)
    {
        var lesson = await unitOfWork.Lessons.GetWithElementsAndCoachById(lessonId);

        if (lesson is null)
            return NotFound();

        if (lesson.Course?.CoachId != await claimsPrincipalService.GetCoachId(User))
            return Forbid();

        var serviceResult = await videoService.ValidateAndAddVideo(lesson, addDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseVideoDto = serviceResult.Data;
        return CreatedAtAction("Get", "CourseVideo", new { id = courseVideoDto.Id }, courseVideoDto);
    }

    #endregion
}
