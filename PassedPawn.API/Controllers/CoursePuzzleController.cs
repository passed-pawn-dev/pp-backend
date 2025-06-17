using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Puzzle;
using PassedPawn.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CoursePuzzleController(IUnitOfWork unitOfWork, IClaimsPrincipalService claimsPrincipalService,
    ICoursePuzzleService puzzleService) : ApiControllerBase
{
    
    [HttpGet("{id:int}")]
    [Authorize(Policy = "require student or coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoursePuzzlesDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single puzzle by id"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var userRole = claimsPrincipalService.IsLoggedInAsStudent(User) ? UserRole.Student : UserRole.Coach ;
        var userId = userRole == UserRole.Student
            ? await claimsPrincipalService.GetStudentId(User)
            : await claimsPrincipalService.GetCoachId(User);
        
        var puzzle = userRole == UserRole.Student 
            ? await unitOfWork.Puzzles.GetOwnedOrInPreviewForStudentAsync(id, userId)
            : await unitOfWork.Puzzles.GetOwnedOrInPreviewForCoachAsync(id, userId);
        
        return puzzle is null ? NotFound() : Ok(puzzle);
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoursePuzzlesDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Updates a puzzle",
        Description = "New puzzle's order can be in the middle of the lesson, so other elements' orders might be modified to account for that."
    )]
    public async Task<IActionResult> UpdatePuzzle(int id, CoursePuzzleUpsertDto upsertDto)
    {
        var lesson = await unitOfWork.Lessons.GetByPuzzleId(id);

        if (lesson is null)
            return NotFound();

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

        var serviceResult = await puzzleService.ValidateAndUpdatePuzzle(lesson, id, upsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var coursePuzzleDto = serviceResult.Data;
        return Ok(coursePuzzleDto);
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a puzzle"
    )]
    public async Task<IActionResult> DeletePuzzle(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByPuzzleId(id);

        if (lesson is null)
            return NotFound();
        
        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

        var coursePuzzle = lesson.Puzzles.Single(puzzle => puzzle.Id == id);
        await puzzleService.DeletePuzzle(lesson, coursePuzzle);
        return NoContent();
    }

    [Authorize(Policy = "require student role")]
    [HttpPost("{puzzleId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerOperation(
        Summary = "Send puzzle solution by Student"
    )]
    public async Task<IActionResult> PostSolution(int puzzleId, SolutionDto solution)
    {
        var serviceResult = await puzzleService.CheckPuzzleSolution(User, puzzleId, solution.Solution);

        if (serviceResult.IsSuccess)
            return Ok(serviceResult.Data);

        return serviceResult.Errors.First() switch
        {
            "Puzzle not found" => NotFound(),
            "Already solved this puzzle" => Conflict("Already solved this puzzle"),
            "Invalid solution" => UnprocessableEntity("Invalid answer"),
            "Invalid number of moves" => UnprocessableEntity("Invalid number of moves"),
            _ => throw new Exception("This shouldn't have happened")
        };
    }
}