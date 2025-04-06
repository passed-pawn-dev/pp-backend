using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Quiz;

namespace PassedPawn.API.Controllers;

public class CourseQuizController(IUnitOfWork unitOfWork, IClaimsPrincipalService claimsPrincipalService) : ApiControllerBase
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

    [HttpPost("{id:int}")]
    public async Task<IActionResult> PostSolution()
    {
        
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByIdAsync(id);

        if (lesson is null)
            return NotFound();

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

    }
}