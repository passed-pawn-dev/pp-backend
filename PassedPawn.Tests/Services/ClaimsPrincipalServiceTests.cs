using System.Security.Claims;
using Moq;
using PassedPawn.BusinessLogic.Services;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.Tests.Services;

public class ClaimsPrincipalServiceTests
{
    private readonly ClaimsPrincipalService _claimsPrincipalService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    
    public ClaimsPrincipalServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(true);
        
        _claimsPrincipalService = new ClaimsPrincipalService(_unitOfWorkMock.Object);
    }

    private static readonly string SampleEmail = "user@example.com";

    private static ClaimsPrincipal SampleClaimsPrincipal()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, SampleEmail)
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        return new ClaimsPrincipal(identity);
    }

    private static Student SampleStudent()
    {
        return new Student
        {
            Id = 1,
            Email = SampleEmail,
            FirstName = "Student",
            LastName = "Test"
        };
    }

    [Fact]
    public async Task GetStudentId_ShouldReturnId_WhenStudentExists()
    {
        // Arrange
        const int expectedId = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetIdByEmail(SampleEmail))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _claimsPrincipalService.GetStudentId(SampleClaimsPrincipal());

        // Assert
        Assert.Equal(expectedId, result);
    }
    
    [Fact]
    public async Task GetStudentId_ShouldThrow_WhenStudentDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetIdByEmail(SampleEmail))
            .ReturnsAsync((int?)null);

        // Act && Assert
        await Assert.ThrowsAsync<Exception>(() => _claimsPrincipalService.GetStudentId(SampleClaimsPrincipal()));
    }
    
    [Fact]
    public async Task GetStudentIdOptional_ShouldReturnId_WhenStudentExists()
    {
        // Arrange
        const int expectedId = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetIdByEmail(SampleEmail))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _claimsPrincipalService.GetStudentIdOptional(SampleClaimsPrincipal());

        // Assert
        Assert.Equal(expectedId, result);
    }
    
    [Fact]
    public async Task GetStudentIdOptional_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetIdByEmail(SampleEmail))
            .ReturnsAsync((int?)null);

        // Act
        var result = await _claimsPrincipalService.GetStudentIdOptional(SampleClaimsPrincipal());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetStudent_ShouldReturnStudent_WhenStudentExists()
    {
        // Arrange
        var student = SampleStudent();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetUserByEmail(SampleEmail))
            .ReturnsAsync(student);

        // Act
        var result = await _claimsPrincipalService.GetStudent(SampleClaimsPrincipal());

        // Assert
        Assert.Equal(student, result);
    }
    
    [Fact]
    public async Task GetStudent_ShouldThrow_WhenStudentDoesExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetUserByEmail(SampleEmail))
            .ReturnsAsync((Student?)null);

        // Act && Assert
        await Assert.ThrowsAsync<Exception>(() => _claimsPrincipalService.GetStudent(SampleClaimsPrincipal()));
    }
    
    [Fact]
    public async Task GetCoachId_ShouldReturnId_WhenStudentExists()
    {
        // Arrange
        const int expectedId = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Coaches.GetUserIdByEmail(SampleEmail))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _claimsPrincipalService.GetCoachId(SampleClaimsPrincipal());

        // Assert
        Assert.Equal(expectedId, result);
    }
    
    [Fact]
    public async Task GetCoachId_ShouldThrow_WhenStudentDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Coaches.GetUserIdByEmail(SampleEmail))
            .ReturnsAsync((int?)null);

        // Act && Assert
        await Assert.ThrowsAsync<Exception>(() => _claimsPrincipalService.GetCoachId(SampleClaimsPrincipal()));
    }
}
