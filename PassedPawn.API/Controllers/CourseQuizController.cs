using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Quiz;

namespace PassedPawn.API.Controllers;

public class CourseQuizController(IUnitOfWork unitOfWork, IClaimsPrincipalService claimsPrincipalService, ICourseQuizService quizService) : ApiControllerBase
{
    // Add Quiz service and inherit from generic class
    // Add Quiz Controller
    // Add quizes to some model to update order (need to check where)
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var quiz = await unitOfWork.Quizzes.GetByIdAsync<CourseQuizDto>(id);
        return quiz is null ? NotFound() : Ok(quiz);
    }
    
    [Authorize(Policy = "require student role")]
    [HttpPost("{id:int}")]
    public async Task<IActionResult> PostSolution()
    {
        return Ok();
    }
    
    // Put doent work inseatd of updating answers it add new to existing ones
    [Authorize(Policy = "require coach role")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, CourseQuizUpsertDto upsertDto)
    {
        var lesson = await unitOfWork.Lessons.GetByQuizId(id);

        if (lesson is null)
            return NotFound();

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();
        
        var serviceResult = await quizService.ValidateAndUpdateQuiz(lesson, id, upsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseQuizDto = serviceResult.Data;
        return Ok(courseQuizDto);

    }
}