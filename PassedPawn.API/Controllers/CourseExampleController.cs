using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Example;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseExampleController(IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseExampleDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single puzzle by id"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var puzzle = await unitOfWork.Examples.GetByIdAsync<CourseExampleDto>(id);
        return puzzle is null ? NotFound() : Ok(puzzle);
    }
}
