using Microsoft.AspNetCore.Mvc;
using Moq;
using PassedPawn.API.Controllers;
using PassedPawn.BusinessLogic.Services;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.Tests.Controllers;

public class StudentControllerTests
{
    private readonly StudentController _studentController;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public StudentControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userServiceMock = new Mock<IUserService>();
        _studentController = new StudentController(_userServiceMock.Object, _unitOfWorkMock.Object);
    }

    private static Student SampleStudent()
    {
        return new Student()
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
        };
    }

    private static StudentDto SampleStudentDto()
    {
        return new StudentDto()
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
        };
    }

    private static StudentUpsertDto SampleStudentUpsertDto()
    {
        return new StudentUpsertDto()
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
        };
    }
    
    [Fact]
    public async Task GetStudent_ShouldReturnOk_WhenStudentExists()
    {
        // Arrange
        const int id = 1;
        var studentDto = SampleStudentDto;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetByIdAsync<StudentDto>(id)).ReturnsAsync(studentDto);

        // Act
        var result = await _studentController.GetStudent(id);
        
        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(studentDto, okObject.Value);
    }

    [Fact]
    public async Task GetStudent_ShouldReturnNotFound_WhenStudentDoesNotExist()
    {
        // Arrange
        const int id = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetByIdAsync<StudentUpsertDto>(id)).ReturnsAsync(null as StudentUpsertDto);
        
        // Act
        var result = await _studentController.GetStudent(id);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task RegisterStudent_ShouldReturnCreatedAtAction_WhenValidationPassed()
    {
        // Arrange
        var studentUpsertDto = SampleStudentUpsertDto();
        var studentDto = SampleStudentDto();
        
        _userServiceMock.Setup(userService => userService.AddStudent(studentUpsertDto)).ReturnsAsync(ServiceResult<StudentDto>.Success(studentDto));

        // Act
        var result = await _studentController.RegisterStudent(studentUpsertDto);
        
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(studentDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task RegisterStudent_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var studentUpsertDto = SampleStudentUpsertDto();
        var errors = new List<string> { "Error Test" };
        _userServiceMock.Setup(userService => userService.AddStudent(studentUpsertDto)).ReturnsAsync(ServiceResult<StudentDto>.Failure(errors));

        // Act
        var result = await _studentController.RegisterStudent(studentUpsertDto);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, badRequestObjectResult.Value);
    }
}