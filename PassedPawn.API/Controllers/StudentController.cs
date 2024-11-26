using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.User.Student;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class StudentController(IUserService userService, IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Registers a new user",
        Description = "This endpoint allows for the registration of a new user. It takes in the details of the student and creates a new record."
    )]
    public async Task<IActionResult> Register(StudentUpsertDto studentUpsertDto)
    {
        // User service 
        ServiceResult<StudentDto> serviceResponse = await userService.AddUser(studentUpsertDto);

        if (!serviceResponse.IsSuccess)
            return BadRequest(serviceResponse.Errors);

        return CreatedAtAction(nameof(Get), new { id = serviceResponse.Data.Id }, serviceResponse.Data);
    }

    // TODO: Protect this route
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var studentDto = await unitOfWork.Students.GetByIdAsync<StudentUpsertDto>(id);

        if (studentDto is null)
            return NotFound();

        return Ok(studentDto);
    }

    // TODO: Rest of CRUD
}