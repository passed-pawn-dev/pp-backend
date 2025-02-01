using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.API.Extensions;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Puzzle;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class PuzzleController(IUnitOfWork unitOfWork, IMapper mapper) : ApiControllerBase
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
    [Authorize]
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
    [HttpPost("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerOperation(
        Summary = "Send puzzle solution by Student"
    )]
    public async Task<IActionResult> PostSolution(int id, [FromBody] string solution)
    {
        var puzzle = await unitOfWork.Puzzles.GetPuzzleById(id);

        if (puzzle is null)
            return NotFound();
        
        var student = await unitOfWork.Students.GetUserByEmail(User.GetUserId())
                      ?? throw new Exception("Coach exists in Keyclock but not in out database");

        if (puzzle.Students.Contains(student))
            return Conflict(new ErrorResponseDto("Already solved this puzzle"));

        if (puzzle.Solution != solution)
            return BadRequest(new ErrorResponseDto("Invalid solution"));
        
        student.Puzzles.Add(puzzle);
        unitOfWork.Students.Update(student);
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}