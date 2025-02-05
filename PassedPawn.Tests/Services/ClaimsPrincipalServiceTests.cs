using System.Security.Claims;
using Moq;
using PassedPawn.BusinessLogic.Services;
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

    [Fact]
    public async Task GetStudentId_ShouldReturnId_WhenStudent()
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
    
}
