using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.User.Student;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class StudentController(IUserService userService, IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
    [SwaggerOperation(
        Summary = "Registers a new student"
    )]
    public async Task<IActionResult> RegisterStudent(StudentUpsertDto studentUpsertDto)
    {
        // User service 
        var serviceResponse = await userService.AddUser(studentUpsertDto);

        if (!serviceResponse.IsSuccess)
            return BadRequest(serviceResponse.Errors);

        return CreatedAtAction(nameof(GetStudent), new { id = serviceResponse.Data.Id }, serviceResponse.Data);
    }

    // TODO: Protect this route
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns student details"
    )]
    public async Task<IActionResult> GetStudent(int id)
    {
        var studentDto = await unitOfWork.Students.GetByIdAsync<StudentUpsertDto>(id);

        if (studentDto is null)
            return NotFound();

        return Ok(studentDto);
    }

    // TODO: Rest of CRUD
}