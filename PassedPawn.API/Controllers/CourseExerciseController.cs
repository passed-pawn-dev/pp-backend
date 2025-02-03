using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.API.Extensions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Exercise;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseExerciseController(IUnitOfWork unitOfWork, IMapper mapper, IPuzzleService puzzleService) : ApiControllerBase
{
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseExerciseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single puzzle by id"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var puzzle = await unitOfWork.Puzzles.GetByIdAsync<CourseExerciseDto>(id);
        return puzzle is null ? NotFound() : Ok(puzzle);
    }

    [Authorize]
    [HttpPost("{puzzleId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerOperation(
        Summary = "Send puzzle solution by Student"
    )]
    public async Task<IActionResult> PostSolution(int puzzleId, SolutionDto solution)
    {
        var serviceResult = await puzzleService.CheckPuzzleSolution(User.GetUserEmail(), puzzleId, solution.Solution);

        if (serviceResult.IsSuccess)
            return Ok(serviceResult.Data);

        return serviceResult.Errors.First() switch
        {
            "Puzzle not found" => NotFound(),
            "Already solved this puzzle" => Conflict("Already solved this puzzle"),
            "Invalid answer" => UnprocessableEntity("Invalid answer"),
            "Invalid number of moves" => UnprocessableEntity("Invalid number of moves"),
            _ => throw new Exception("This shouldn't have happened")
        };
    }
}