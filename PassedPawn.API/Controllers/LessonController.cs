using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.API.Controllers;

public class LessonController(IUnitOfWork unitOfWork, ICourseService courseService) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLesson(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByIdAsync<LessonDto>(id);

        if (lesson is null)
            return NotFound();

        return Ok(lesson);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateLesson(int id, LessonUpsertDto lessonUpsertDto)
    {
        Course? course = await unitOfWork.Courses.GetByLessonId(id);

        if (course is null)
            return NotFound();

        ServiceResult<LessonDto> serviceResult =
            await courseService.ValidateAndUpdateLesson(course, id, lessonUpsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        LessonDto? lessonDto = serviceResult.Data;
        return Ok(lessonDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        Lesson? lesson = await unitOfWork.Lessons.GetByIdAsync(id);

        if (lesson is null)
            return NotFound();

        unitOfWork.Lessons.Delete(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }
}