using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.API.Controllers;

public class LessonController(IUnitOfWork unitOfWork, IMapper mapper) : ApiControllerBase
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
        var lesson = await unitOfWork.Lessons.GetByIdAsync(id);

        if (lesson is null)
            return NotFound();

        mapper.Map(lessonUpsertDto, lesson);
        unitOfWork.Lessons.Update(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        var lessonDto = mapper.Map<LessonDto>(lesson);
        return Ok(lessonDto);
    }

    [HttpDelete("{id:int}")]
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
}
