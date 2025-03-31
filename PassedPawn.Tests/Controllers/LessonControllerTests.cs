using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PassedPawn.API.Controllers;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.Tests.Controllers;

public class LessonControllerTests
{
    private readonly Mock<ICourseService> _courseServiceMock;
    private readonly LessonController _lessonController;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IClaimsPrincipalService> _claimsPrincipalServiceMock;

    public LessonControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseServiceMock = new Mock<ICourseService>();
        _claimsPrincipalServiceMock = new Mock<IClaimsPrincipalService>();
        _lessonController = new LessonController(_unitOfWorkMock.Object, _courseServiceMock.Object,
            _claimsPrincipalServiceMock.Object);
    }

    private static Course SampleCourse()
    {
        return new Course
        {
            CoachId = 1,
            Title = "Test",
            Description = "Test",
            Lessons =
            [
                new Lesson
                {
                    Id = 1,
                    LessonNumber = 1
                }
            ]
        };
    }

    private static LessonDto SampleLessonDto()
    {
        return new LessonDto
        {
            LessonNumber = 1
        };
    }

    private static LessonUpsertDto SampleLessonUpsertDto()
    {
        return new LessonUpsertDto
        {
            LessonNumber = 1
        };
    }

    [Fact]
    public async Task GetLesson_ShouldReturnOk_WhenCourseExists()
    {
        // Arrange
        const int id = 1;
        var lessonDto = SampleLessonDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Lessons.GetByIdAsync<LessonDto>(id))
            .ReturnsAsync(lessonDto);

        // Act
        var result = await _lessonController.GetLesson(id);

        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lessonDto, okObject.Value);
    }

    [Fact]
    public async Task GetLesson_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        const int id = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Lessons.GetByIdAsync<LessonDto>(id))
            .ReturnsAsync((LessonDto?)null);

        // Act
        var result = await _lessonController.GetLesson(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateLesson_ShouldReturnOk_WhenValidationPasses()
    {
        // Arrange
        const int id = 1;
        var lessonDto = SampleLessonDto();
        var lessonUpsertDto = SampleLessonUpsertDto();
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndUpdateLesson(course, id, lessonUpsertDto))
            .ReturnsAsync(ServiceResult<LessonDto>.Success(lessonDto));
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _lessonController.UpdateLesson(id, lessonUpsertDto);

        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lessonDto, okObject.Value);
    }

    [Fact]
    public async Task UpdateLesson_ShouldReturnNotFound_WhenLessonDoesNotExist()
    {
        // Arrange
        const int id = 1;
        var lessonUpsertDto = SampleLessonUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _lessonController.UpdateLesson(id, lessonUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateLesson_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        const int id = 1;
        var errors = new List<string> { "Test error" };
        var lessonUpsertDto = SampleLessonUpsertDto();
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndUpdateLesson(course, id, lessonUpsertDto))
            .ReturnsAsync(ServiceResult<LessonDto>.Failure(errors));
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _lessonController.UpdateLesson(id, lessonUpsertDto);

        // Assert
        var okObject = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, okObject.Value);
    }

    [Fact]
    public async Task DeleteLesson_ShouldReturnNoContent_WhenLessonExists()
    {
        // Arrange
        const int id = 1;
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync(course);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(true);
        _unitOfWorkMock.Setup(unitOfWorkMock => unitOfWorkMock.Lessons.Delete(It.IsAny<Lesson>()));
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _lessonController.DeleteLesson(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteLesson_ShouldReturnNotFound_WhenLessonDoesNotExist()
    {
        // Arrange
        const int id = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _lessonController.DeleteLesson(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}