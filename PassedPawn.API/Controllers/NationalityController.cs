using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Nationality;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

// TODO: Protect this controller
public class NationalityController(IUnitOfWork unitOfWork, IMapper mapper) : ApiControllerBase
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

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalityDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns a single nationality by id"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var nationalityDto = await unitOfWork.Nationalities.GetByIdAsync<NationalityDto>(id);

        if (nationalityDto is null)
            return NotFound();

        return Ok(nationalityDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalityDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<string>))]
    [SwaggerOperation(
        Summary = "Creates a nationality"
    )]
    public async Task<IActionResult> Create(NationalityUpsertDto nationalityUpsertDto)
    {
        var nationality = mapper.Map<Nationality>(nationalityUpsertDto);
        unitOfWork.Nationalities.Add(nationality);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save");

        var nationalityDto = mapper.Map<NationalityDto>(nationality);
        return CreatedAtAction(nameof(Get), new { id = nationalityDto.Id }, nationalityDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalityDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<string>))]
    [SwaggerOperation(
        Summary = "Updates a nationality"
    )]
    public async Task<IActionResult> Update(int id, NationalityUpsertDto nationalityUpsertDto)
    {
        var nationality = await unitOfWork.Nationalities.GetByIdAsync(id);

        if (nationality is null)
            return NotFound();

        mapper.Map(nationalityUpsertDto, nationality);
        unitOfWork.Nationalities.Update(nationality);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save");

        var nationalityDto = mapper.Map<NationalityDto>(nationality);
        return Ok(nationalityDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a nationality"
    )]
    public async Task<IActionResult> Delete(int id)
    {
        var nationality = await unitOfWork.Nationalities.GetByIdAsync(id);

        if (nationality is null)
            return NotFound();

        unitOfWork.Nationalities.Delete(nationality);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to delete");

        return NoContent();
    }
}