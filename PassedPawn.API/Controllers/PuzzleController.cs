using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.API.Extensions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Puzzle;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class PuzzleController(IUnitOfWork unitOfWork, IMapper mapper, IPuzzleService puzzleService) : ApiControllerBase
{
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PuzzleDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single puzzle by id"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var puzzle = await unitOfWork.Puzzles.GetByIdAsync<PuzzleDto>(id);
        return puzzle is null ? NotFound() : Ok(puzzle);
    }
    
    // Only for coach
    //TODO roles
    [Authorize(Policy = "require coach role")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PuzzleDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Create new puzzle by Coach"
    )]
    public async Task<IActionResult> Post(PuzzleUpsertDto dto)
    {
        var email = User.GetUserId();
        var puzzle = mapper.Map<Puzzle>(dto);
        puzzle.CoachId = await unitOfWork.Coaches.GetUserIdByEmail(email)
                         ?? throw new Exception("Coach exists in Keyclock but not in out database");
        
        unitOfWork.Puzzles.Add(puzzle);
        await unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = puzzle.Id }, puzzle);
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
        var serviceResult = await puzzleService.CheckPuzzleSolution(User.GetUserId(), puzzleId, solution.Solution);

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