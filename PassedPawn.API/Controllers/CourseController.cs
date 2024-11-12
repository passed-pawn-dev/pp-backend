using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.API.Controllers;

public class CourseController(IUnitOfWork unitOfWork, IMapper mapper) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCourses() // TODO: Add filters
    {
        var courses = await unitOfWork.Courses.GetAllAsync<CourseDto>();
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
        var course = mapper.Map<Course>(courseUpsertDto);
        unitOfWork.Courses.Add(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        var courseDto = mapper.Map<CourseDto>(course);
        return CreatedAtAction(nameof(GetCourse), new { id = courseDto.Id }, courseDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCourse(int id, CourseUpsertDto courseUpsertDto)
    {
        var course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();

        mapper.Map(courseUpsertDto, course);
        unitOfWork.Courses.Update(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        var courseDto = mapper.Map<CourseDto>(course);
        return Ok(courseDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();
        
        unitOfWork.Courses.Delete(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }

    [HttpGet("{id:int}/lessons")]
    public async Task<IActionResult> GetLessons(int id)
    {
        var lessons = await unitOfWork.Lessons
            .GetAllWhereAsync<LessonDto>(lesson => lesson.CourseId == id);

        return Ok(lessons);
    }

    [HttpPost("{id:int}/lesson")]
    public async Task<IActionResult> AddLesson(int id, LessonUpsertDto lessonUpsertDto)
    {
        var course = await unitOfWork.Courses.GetByIdAsync(id);

        if (course is null)
            return NotFound();

        var lesson = mapper.Map<Lesson>(lessonUpsertDto);
        course.Lessons.Add(lesson);
        unitOfWork.Courses.Update(course);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        var lessonDto = mapper.Map<LessonDto>(lesson);

        return CreatedAtAction("GetLesson", "Lesson", new { id = lesson.Id }, lessonDto);
    }
}
