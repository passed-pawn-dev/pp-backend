using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Example;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseExampleController(IUnitOfWork unitOfWork, ICourseExampleService exampleService,
    IClaimsPrincipalService claimsPrincipalService) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [Authorize(Policy = "require student role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseExampleDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single example by id"
    )]
    public async Task<IActionResult> Get(int id)
    {
        var userId = await claimsPrincipalService.GetStudentId(User);
        var puzzle = await unitOfWork.Examples.GetOwnedOrInPreviewAsync(id, userId);
        return puzzle is null ? NotFound() : Ok(puzzle);
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseExampleDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Updates an example",
        Description = "New example's order can be in the middle of the lesson, so other elements' orders might be modified to account for that."
    )]
    public async Task<IActionResult> UpdateExample(int id, CourseExampleUpsertDto upsertDto)
    {
        var lesson = await unitOfWork.Lessons.GetByExampleId(id);

        if (lesson is null)
            return NotFound();

        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

        var serviceResult = await exampleService.ValidateAndUpdateExample(lesson, id, upsertDto);

        if (!serviceResult.IsSuccess)
            return BadRequest(serviceResult.Errors);

        var courseExampleDto = serviceResult.Data;
        return Ok(courseExampleDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "require coach role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes an example"
    )]
    public async Task<IActionResult> DeleteExample(int id)
    {
        var lesson = await unitOfWork.Lessons.GetByExampleId(id);

        if (lesson is null)
            return NotFound();
        
        var coachId = await claimsPrincipalService.GetCoachId(User);

        if (lesson.Course?.CoachId != coachId)
            return Forbid();

        var courseExample = lesson.Examples.Single(example => example.Id == id);
        await exampleService.DeleteExample(lesson, courseExample);
        return NoContent();
    }
}
