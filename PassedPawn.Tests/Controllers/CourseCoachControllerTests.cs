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

namespace PassedPawn.Tests.Controllers;

public class CourseCoachControllerTests
{
    private const int UserId = 1;
    private readonly CourseCoachController _courseCoachController;
    private readonly Mock<ICourseService> _courseServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CourseCoachControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var claimPrincipalServiceMock = new Mock<IClaimsPrincipalService>();
        claimPrincipalServiceMock.Setup(claimsPrincipalService =>
                claimsPrincipalService.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(UserId);
        _courseServiceMock = new Mock<ICourseService>();
        _courseCoachController = new CourseCoachController(_unitOfWorkMock.Object, _courseServiceMock.Object,
            claimPrincipalServiceMock.Object);
    }

    #region DTOs
    
    private static Course SampleCourse()
    {
        return new Course
        {
            CoachId = UserId,
            Title = "Test",
            Description = "Test"
        };
    }
    
    private static LessonDto SampleLessonDto()
    {
        return new LessonDto
        {
            Title = "Title 1",
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
    
    private static CourseDto SampleCourseDto()
    {
        return new CourseDto
        {
            Id = 1,
            Title = "Mastering Endgames",
            Description = "An advanced course focused on chess endgame strategies.",
            EloRangeStart = 1800,
            EloRangeEnd = 2200,
            CoachName = "GM John Doe",
            AverageScore = 4.7m
        };
    }

    private static CourseUpsertDto SampleCourseUpsertDto()
    {
        return new CourseUpsertDto
        {
            Title = "Test",
            Description = "Test"
        };
    }
    
    #endregion
    
    [Fact]
    public async Task AddLesson_ShouldReturnCreatedAtAction_WhenValidationPasses()
    {
        // Arrange
        const int id = 1;
        var lessonDto = SampleLessonDto();
        var lessonUpsertDto = SampleLessonUpsertDto();
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetWithLessonsById(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndAddLesson(course, lessonUpsertDto))
            .ReturnsAsync(ServiceResult<LessonDto>.Success(lessonDto));

        // Act
        var result = await _courseCoachController.AddLesson(id, lessonUpsertDto);

        // Assert
        var createAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(lessonDto, createAtActionResult.Value);
    }

    [Fact]
    public async Task AddLesson_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        const int id = 1;
        var lessonUpsertDto = SampleLessonUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetWithLessonsById(id))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseCoachController.AddLesson(id, lessonUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddLesson_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        const int id = 1;
        var errors = new List<string> { "Test error" };
        var lessonUpsertDto = SampleLessonUpsertDto();
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetWithLessonsById(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndAddLesson(course, lessonUpsertDto))
            .ReturnsAsync(ServiceResult<LessonDto>.Failure(errors));

        // Act
        var result = await _courseCoachController.AddLesson(id, lessonUpsertDto);

        // Assert
        var createAtActionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, createAtActionResult.Value);
    }
    
    [Fact]
    public async Task CreateCourse_ShouldReturnCreateAtAction_WhenValidationPasses()
    {
        // Arrange
        var courseUpsertDto = SampleCourseUpsertDto();
        var courseDto = SampleCourseDto();
        _courseServiceMock.Setup(courseService => courseService.ValidateAndAddCourse(1, courseUpsertDto))
            .ReturnsAsync(ServiceResult<CourseDto>.Success(courseDto));

        // Act
        var result = await _courseCoachController.CreateCourse(courseUpsertDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(courseDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task CreateCourse_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var courseUpsertDto = SampleCourseUpsertDto();
        var errors = new List<string> { "Test error" };
        _courseServiceMock.Setup(courseService => courseService.ValidateAndAddCourse(1, courseUpsertDto))
            .ReturnsAsync(ServiceResult<CourseDto>.Failure(errors));

        // Act
        var result = await _courseCoachController.CreateCourse(courseUpsertDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateCourse_ShouldReturnOk_WhenValidationPasses()
    {
        // Arrange
        const int id = 1;
        var courseUpsertDto = SampleCourseUpsertDto();
        var courseDto = SampleCourseDto();
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndUpdateCourse(course, courseUpsertDto))
            .ReturnsAsync(ServiceResult<CourseDto>.Success(courseDto));

        // Act
        var result = await _courseCoachController.UpdateCourse(id, courseUpsertDto);

        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(courseDto, okObject.Value);
    }

    [Fact]
    public async Task UpdateCourse_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        const int id = 1;
        var courseUpsertDto = SampleCourseUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseCoachController.UpdateCourse(id, courseUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateCourse_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        const int id = 1;
        var courseUpsertDto = SampleCourseUpsertDto();
        var errors = new List<string> { "Test error" };
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.ValidateAndUpdateCourse(course, courseUpsertDto))
            .ReturnsAsync(ServiceResult<CourseDto>.Failure(errors));

        // Act
        var result = await _courseCoachController.UpdateCourse(id, courseUpsertDto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, badRequest.Value);
    }

    [Fact]
    public async Task DeleteCourse_ShouldReturnNoContent_WhenCourseExists()
    {
        // Arrange
        const int id = 1;
        var course = SampleCourse();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync(course);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _courseCoachController.DeleteCourse(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCourse_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        const int id = 1;
        var courseUpsertDto = SampleCourseUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseCoachController.UpdateCourse(id, courseUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}