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

    public LessonControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseServiceMock = new Mock<ICourseService>();
        _lessonController = new LessonController(_unitOfWorkMock.Object, _courseServiceMock.Object);
    }

    private static Course SampleCourse()
    {
        return new Course
        {
            Title = "Test",
            Description = "Test"
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

    private static Lesson SampleLesson()
    {
        return new Lesson
        {
            LessonNumber = 1
        };
    }

    [Fact]
    public async Task GetLesson_ShouldReturnOk_WhenCourseExists()
    {
        // Arrange
        const int id = 1;
        LessonDto lessonDto = SampleLessonDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Lessons.GetByIdAsync<LessonDto>(id))
            .ReturnsAsync(lessonDto);

        // Act
        IActionResult result = await _lessonController.GetLesson(id);

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
        IActionResult result = await _lessonController.GetLesson(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateLesson_ShouldReturnOk_WhenValidationPasses()
    {
        // Arrange
        const int id = 1;
        LessonDto lessonDto = SampleLessonDto();
        LessonUpsertDto lessonUpsertDto = SampleLessonUpsertDto();
        Course course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndUpdateLesson(course, id, lessonUpsertDto))
            .ReturnsAsync(ServiceResult<LessonDto>.Success(lessonDto));

        // Act
        IActionResult result = await _lessonController.UpdateLesson(id, lessonUpsertDto);

        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lessonDto, okObject.Value);
    }

    [Fact]
    public async Task UpdateLesson_ShouldReturnNotFound_WhenLessonDoesNotExist()
    {
        // Arrange
        const int id = 1;
        LessonUpsertDto lessonUpsertDto = SampleLessonUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync((Course?)null);

        // Act
        IActionResult result = await _lessonController.UpdateLesson(id, lessonUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateLesson_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        const int id = 1;
        var errors = new List<string> { "Test error" };
        LessonUpsertDto lessonUpsertDto = SampleLessonUpsertDto();
        Course course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByLessonId(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndUpdateLesson(course, id, lessonUpsertDto))
            .ReturnsAsync(ServiceResult<LessonDto>.Failure(errors));

        // Act
        IActionResult result = await _lessonController.UpdateLesson(id, lessonUpsertDto);

        // Assert
        var okObject = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, okObject.Value);
    }

    [Fact]
    public async Task DeleteLesson_ShouldReturnNoContent_WhenLessonExists()
    {
        // Arrange
        const int id = 1;
        Lesson lesson = SampleLesson();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Lessons.GetByIdAsync(id))
            .ReturnsAsync(lesson);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        IActionResult result = await _lessonController.DeleteLesson(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteLesson_ShouldReturnNotFound_WhenLessonDoesNotExist()
    {
        // Arrange
        const int id = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Lessons.GetByIdAsync(id))
            .ReturnsAsync((Lesson?)null);

        // Act
        IActionResult result = await _lessonController.DeleteLesson(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}