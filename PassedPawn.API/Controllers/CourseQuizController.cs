using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Quiz;
using PassedPawn.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseQuizController(IUnitOfWork unitOfWork, IClaimsPrincipalService claimsPrincipalService,
    ICourseQuizService quizService) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [Authorize(Policy = "require student or coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseQuizDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get Course Quiz by id"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var userRole = claimsPrincipalService.IsLoggedInAsStudent(User) ? UserRole.Student : UserRole.Coach ;
        var userId = userRole == UserRole.Student
            ? await claimsPrincipalService.GetStudentId(User)
            : await claimsPrincipalService.GetCoachId(User);
        
        var quiz = userRole == UserRole.Student 
            ? await unitOfWork.Quizzes.GetOwnedOrInPreviewForStudentAsync(id, userId)
            : await unitOfWork.Quizzes.GetOwnedOrInPreviewForCoachAsync(id, userId);
        
        return quiz is null ? NotFound() : Ok(quiz);
    }
    
    [Authorize(Policy = "require coach role")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseQuizDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Update quiz",
        Description = "New quiz's order can be in the middle of the lesson, so other elements' orders might be modified to account for that."
    )]
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
    
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a quiz"
    )]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByQuizId(id);

        if (lesson is null)
            return NotFound();
        
        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

        var courseQuiz = lesson.Quizzes.Single(quiz => quiz.Id == id);
        await quizService.DeleteQuiz(lesson, courseQuiz);
        return NoContent();
    }
}