using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PassedPawn.API.Controllers;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;

namespace PassedPawn.Tests.Controllers;

public class CourseControllerTests
{
    private readonly CourseController _courseController;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CourseControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseController = new CourseController(_unitOfWorkMock.Object);
    }

    private static Course SampleCourse()
    {
        return new Course
        {
            CoachId = 1,
            Title = "Test",
            Description = "Test"
        };
    }

    [Fact]
    public async Task GetReviews_ShouldReturnOk()
    {
        // Arrange
        const int id = 1;
        var courseReviewDtoList = new List<CourseReviewDto>
        {
            new()
        };

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews
                .GetAllWhereAsync<CourseReviewDto>(review => review.CourseId == id))
            .ReturnsAsync(courseReviewDtoList);

        // Act
        var result = await _courseController.GetReviews(id);

        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(courseReviewDtoList, okObject.Value);
    }
}