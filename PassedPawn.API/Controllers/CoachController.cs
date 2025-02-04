using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.User.Coach;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CoachController(IUserService userService, IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CoachDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Registers a new coach"
    )]
    public async Task<IActionResult> Register(CoachUpsertDto coachUpsertDto)
    {
        var serviceResponse = await userService.AddCoach(coachUpsertDto);

        if (!serviceResponse.IsSuccess)
            return BadRequest(serviceResponse.Errors);

        return CreatedAtAction(nameof(Get), new { id = serviceResponse.Data.Id }, serviceResponse.Data);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoachDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns coach details"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var coachDto = await unitOfWork.Students.GetByIdAsync<CoachDto>(id);

        if (coachDto is null)
            return NotFound();

        return Ok(coachDto);
    }
}