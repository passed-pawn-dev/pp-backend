using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Exceptions;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.User.Coach;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CoachController(IUserService userService, IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpGet("{id:int}/profile")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoachProfileDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Returns coach's profile")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var coach = await unitOfWork.Coaches.GetByIdAsync<CoachProfileDto>(id);
        return coach is null ? NotFound() : Ok(coach);
    }
    
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

    [HttpPatch("pfp")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(Summary = "Adds or updates pfp")]
    public async Task<IActionResult> UploadPfp([FromForm] CoachPfpDto pfpDto,
        IClaimsPrincipalService claimsPrincipalService, ICloudinaryService cloudinaryService)
    {
        var userId = await claimsPrincipalService.GetCoachId(User);
        var coach = await unitOfWork.Coaches.GetWithPhotoById(userId);

        if (coach is null)
            throw new Exception("Coach is null");

        var uploadResult = await cloudinaryService.UploadPhotoAsync(pfpDto.Pfp!);

        if (uploadResult.Error is not null)
            throw new CloudinaryException(uploadResult.Error.Message);

        if (coach.Photo is null)
        {
            coach.Photo = new Photo
            {
                Url = uploadResult.Url.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };
        }
        else
        {
            _ = cloudinaryService.DeletePhotoAsync(coach.Photo.PublicId); // Fire and forget
            coach.Photo.Url = uploadResult.Url.AbsoluteUri;
            coach.Photo.PublicId = uploadResult.PublicId;
        }

        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("pfp")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Deletes a pfp")]
    public async Task<IActionResult> DeletePfp(IClaimsPrincipalService claimsPrincipalService,
        ICloudinaryService cloudinaryService)
    {
        var userId = await claimsPrincipalService.GetCoachId(User);
        var coach = await unitOfWork.Coaches.GetWithPhotoById(userId);

        if (coach is null)
            throw new Exception("Coach is null");

        if (coach.Photo is null)
            return NotFound(new { message = "User has no photo" });

        _ = cloudinaryService.DeletePhotoAsync(coach.Photo.PublicId); // Fire and forget
        unitOfWork.Photos.Delete(coach.Photo);
        coach.Photo = null;
        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}