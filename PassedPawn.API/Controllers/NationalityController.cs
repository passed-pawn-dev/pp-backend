using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Nationality;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class NationalityController(IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NationalityDto>))]
    [SwaggerOperation(
        Summary = "Returns all nationalities"
    )]
    public async Task<IActionResult> GetAll()
    {
        var nationalities = await unitOfWork.Nationalities.GetAllAsync<NationalityDto>();
        return Ok(nationalities);
    }
}