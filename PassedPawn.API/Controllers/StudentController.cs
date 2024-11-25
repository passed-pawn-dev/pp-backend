using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.API.Controllers;

public class StudentController(IUserService userService, IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpPost("register")]
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