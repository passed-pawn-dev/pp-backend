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
    private readonly Mock<ICourseService> _courseServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IClaimsPrincipalService> _claimsPrincipalServiceMock;

    public CourseControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseServiceMock = new Mock<ICourseService>();
        _claimsPrincipalServiceMock = new Mock<IClaimsPrincipalService>();
        _courseController = new CourseController(_unitOfWorkMock.Object,
            _courseServiceMock.Object, _claimsPrincipalServiceMock.Object);
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

    private static CourseDto SampleCourseDto()
    {
        return new CourseDto
        {
            Id = 1,
            Title = "Mastering Endgames",
            Description = "An advanced course focused on chess endgame strategies.",
            EloRageStart = 1800,
            EloRangeEnd = 2200,
            CoachName = "GM John Doe",
            AverageScore = 4.7
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
    public async Task CreateCourse_ShouldReturnCreateAtAction_WhenValidationPasses()
    {
        // Arrange
        var courseUpsertDto = SampleCourseUpsertDto();
        var courseDto = SampleCourseDto();
        _courseServiceMock.Setup(courseService => courseService.ValidateAndAddCourse(1, courseUpsertDto))
            .ReturnsAsync(ServiceResult<CourseDto>.Success(courseDto));
        _claimsPrincipalServiceMock.Setup(course => course.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.CreateCourse(courseUpsertDto);

        // Assert
        var createdAtActionResult = Assert.IsType<OkObjectResult>(result);
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
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.CreateCourse(courseUpsertDto);

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
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.UpdateCourse(id, courseUpsertDto);

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
        var result = await _courseController.UpdateCourse(id, courseUpsertDto);

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
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.UpdateCourse(id, courseUpsertDto);

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
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.DeleteCourse(id);

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
        var result = await _courseController.UpdateCourse(id, courseUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetLessons_ShouldReturnOk()
    {
        // Arrange
        const int id = 1;
        var lessonDtoList = new List<LessonDto>
        {
            SampleLessonDto()
        };
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Lessons.GetUserLessons(It.IsAny<int>(), id))
            .ReturnsAsync(lessonDtoList);

        // Act
        var result = await _courseController.GetLessons(id);

        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(lessonDtoList, okObject.Value);
    }

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
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.AddLesson(id, lessonUpsertDto);

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
        var result = await _courseController.AddLesson(id, lessonUpsertDto);

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
        _claimsPrincipalServiceMock.Setup(service => service.GetCoachId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.AddLesson(id, lessonUpsertDto);

        // Assert
        var createAtActionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, createAtActionResult.Value);
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

    [Fact]
    public async Task AddReview_ShouldReturnCreatedAtAction_WhenCourseExists()
    {
        // Arrange
        const int id = 1;
        var course = new Course { Title = "Test", Description = "Test" };
        var courseReviewDto = new CourseReviewDto();
        var reviewUpsertDto = new CourseReviewUpsertDto();

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync(course);
        _courseServiceMock.Setup(courseService => courseService.AddReview(1, course, reviewUpsertDto))
            .ReturnsAsync(courseReviewDto);
        _claimsPrincipalServiceMock.Setup(service => service.GetStudentId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(1);

        // Act
        var result = await _courseController.AddReview(id, reviewUpsertDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(courseReviewDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task AddReview_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        const int id = 1;
        var reviewUpsertDto = new CourseReviewUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseController.AddReview(id, reviewUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}