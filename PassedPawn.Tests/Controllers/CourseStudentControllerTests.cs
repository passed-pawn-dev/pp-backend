using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PassedPawn.API.Controllers;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Review;

namespace PassedPawn.Tests.Controllers;

public class CourseStudentControllerTests
{
    private const int UserId = 1;
    private readonly CourseStudentController _courseStudentController;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CourseStudentControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var claimPrincipalServiceMock = new Mock<IClaimsPrincipalService>();
        claimPrincipalServiceMock.Setup(claimsPrincipalService =>
                claimsPrincipalService.GetStudentId(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(UserId);
        claimPrincipalServiceMock.Setup(claimsPrincipalService =>
                claimsPrincipalService.GetStudentIdOptional(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(UserId);
        _courseStudentController = new CourseStudentController(_unitOfWorkMock.Object,
            claimPrincipalServiceMock.Object);
    }

    #region DTOs
    
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

    private static BoughtCourseDto SampleBoughtCourseDto()
    {
        return new BoughtCourseDto
        {
            Id = 101,
            Title = "Opening Repertoire for White",
            Description = "Learn solid and aggressive openings as White, suitable for club players.",
            EloRangeStart = 1400,
            EloRangeEnd = 1800,
            CoachName = "IM Jane Smith",
            PictureUrl = "https://example.com/images/opening-repertoire.jpg"
        };
    }

    private static NonBoughtCourseDetailsDto SampleNonBoughtCourseDetailsDto()
    {
        return new NonBoughtCourseDetailsDto
        {
            Id = 202,
            Title = "Advanced Tactics and Combinations",
            Description = "Sharpen your tactical vision with complex combinations and traps.",
            ReleaseDate = new DateOnly(2024, 9, 15),
            Coach = new NonBoughtCourseDetailsCoachDto
            {
                Name = "GM Alexei Romanov",
                ChessTitle = "Grandmaster",
                CreatedCoursesCount = 12,
                Description = "A renowned coach known for his creative tactical training methods."
            },
            PuzzleCount = 150,
            VideoCount = 20,
            QuizCount = 5,
            ExampleCount = 10,
            Language = "English",
            EloRangeStart = 1600,
            EloRangeEnd = 2100,
            TotalVideoCount = 25,
            ReviewCount = 73,
            AverageScore = 4.5,
            Price = 29.99f
        };
    }

    private static BoughtCourseDetailsDto SampleBoughtCourseDetailsDto()
    {
        return new BoughtCourseDetailsDto
        {
            Id = 303,
            Title = "Strategic Planning in Chess",
            Description = "Learn how to create and execute winning plans in all phases of the game.",
            Lessons = new List<BoughtCourseDetailsLessonDto>
            {
                new()
                {
                    Id = 1,
                    LessonNumber = 1,
                    Title = "Title 1",
                    Quizzes = new List<BoughtCourseDetailsLessonElementSlimDto>
                    {
                        new()
                        {
                            Id = 101,
                            Title = "Opening Strategy Quiz",
                            Order = 1,
                            Completed = true
                        }
                    },
                    Puzzles = new List<BoughtCourseDetailsLessonElementSlimDto>
                    {
                        new()
                        {
                            Id = 102,
                            Title = "Pawn Structure Puzzle",
                            Order = 2,
                            Completed = false
                        }
                    },
                    Examples = new List<BoughtCourseDetailsLessonElementSlimDto>
                    {
                        new()
                        {
                            Id = 103,
                            Title = "Capablanca’s Plan Execution",
                            Order = 3,
                            Completed = true
                        }
                    },
                    Videos = new List<BoughtCourseDetailsLessonElementSlimDto>
                    {
                        new()
                        {
                            Id = 104,
                            Title = "Introduction to Strategic Thinking",
                            Order = 4,
                            Completed = true
                        }
                    }
                },
                new()
                {
                    Id = 2,
                    LessonNumber = 2,
                    Title = "Title 2",
                    Quizzes = [],
                    Puzzles = [],
                    Examples = [],
                    Videos = new List<BoughtCourseDetailsLessonElementSlimDto>
                    {
                        new()
                        {
                            Id = 105,
                            Title = "Advanced Planning Techniques",
                            Order = 1,
                            Completed = false
                        }
                    }
                }
            }
        };
    }
    
    #endregion

    [Fact]
    public async Task GetAllCourses_ShouldReturnCourses()
    {
        // Arrange
        var courseDtos = new List<CourseDto> { SampleCourseDto() };
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetAllAsync(UserId))
            .ReturnsAsync(courseDtos);

        // Act
        var result = await _courseStudentController.GetAllCourses();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(courseDtos, okObjectResult.Value);
    }

    [Fact]
    public async Task GetAllBoughtCourses_ShouldReturnUserCourses()
    {
        // Arrange
        var courseDtos = new List<BoughtCourseDto> { SampleBoughtCourseDto() };
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetStudentCourses(UserId))
            .ReturnsAsync(courseDtos);

        // Act
        var result = await _courseStudentController.GetAllBoughtCourses();
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(courseDtos, okObjectResult.Value);
    }

    [Fact]
    public async Task GetCourseDetails_ShouldReturnCourse_IfExists()
    {
        // Arrange
        const int id = 1;
        var courseDto = SampleNonBoughtCourseDetailsDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync<NonBoughtCourseDetailsDto>(id))
            .ReturnsAsync(courseDto);
        
        // Act
        var result = await _courseStudentController.GetCourseDetails(id);
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(courseDto, okObjectResult.Value);
    }
    
    [Fact]
    public async Task GetCourseDetails_ShouldReturnNotFound_IfDoesNotExist()
    {
        // Arrange
        const int id = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync<NonBoughtCourseDetailsDto>(id))
            .ReturnsAsync((NonBoughtCourseDetailsDto?)null);
        
        // Act
        var result = await _courseStudentController.GetCourseDetails(id);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetBoughtCourseDetails_ShouldReturnBoughtCourse_IfExists()
    {
        // Arrange
        const int id = 1;
        var courseDto = SampleBoughtCourseDetailsDto();
        
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetStudentCourse(UserId, id))
            .ReturnsAsync(courseDto);
        
        // Act
        var result = await _courseStudentController.GetBoughtCourseDetails(id);
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(courseDto, okObjectResult.Value);
    }
    
    [Fact]
    public async Task GetBoughtCourseDetails_ShouldReturnNotFound_IfDoesNotExist()
    {
        // Arrange
        const int userId = 1;
        const int id = 1;
        
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Students.GetStudentCourse(userId, id))
            .ReturnsAsync((BoughtCourseDetailsDto?)null);
        
        // Act
        var result = await _courseStudentController.GetBoughtCourseDetails(id);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
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
        var courseServiceMock = new Mock<ICourseService>();
        courseServiceMock.Setup(courseService => courseService.AddReview(1, course, reviewUpsertDto))
            .ReturnsAsync(courseReviewDto);

        // Act
        var result = await _courseStudentController.AddReview(id, courseServiceMock.Object, reviewUpsertDto);

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
        var courseServiceMock = new Mock<ICourseService>();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.GetByIdAsync(id))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _courseStudentController.AddReview(id, courseServiceMock.Object, reviewUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
