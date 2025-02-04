using AutoMapper;
using Moq;
using PassedPawn.API.Configuration;
using PassedPawn.BusinessLogic.Services;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.Tests.Services;

public class CourseServiceTests
{
    private readonly CourseService _courseService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CourseServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(true);

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
        var mapper = new Mapper(config);
        _courseService = new CourseService(_unitOfWorkMock.Object, mapper);
    }

    private static Course SampleCourse()
    {
        return new Course
        {
            CoachId = 1,
            Title = "Test",
            Description = "Test",
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = 1,
                    LessonNumber = 1
                },
                new()
                {
                    Id = 2,
                    LessonNumber = 2
                },
                new()
                {
                    Id = 3,
                    LessonNumber = 3
                }
            }
        };
    }

    private static CourseUpsertDto SampleCourseDto(IEnumerable<LessonUpsertDto> lessons)
    {
        return new CourseUpsertDto
        {
            Title = "Test",
            Description = "Test",
            Lessons = lessons
        };
    }

    private static LessonUpsertDto SampleLessonDto(int lessonNumber)
    {
        return new LessonUpsertDto
        {
            LessonNumber = lessonNumber
        };
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1, 3, 2)]
    [InlineData(2, 1, 3)]
    [InlineData(2, 3, 1)]
    [InlineData(3, 1, 2)]
    [InlineData(3, 2, 1)]
    public async Task ValidateAndAddCourse_ShouldPassValidation(int course1, int course2, int course3)
    {
        // Arrange
        var lessons = new List<LessonUpsertDto>
        {
            SampleLessonDto(course1), SampleLessonDto(course2), SampleLessonDto(course3)
        };

        var course = SampleCourseDto(lessons);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Add(It.IsAny<Course>()));

        // Act
        var result = await _courseService.ValidateAndAddCourse(1, course);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(1, 3, 4)]
    [InlineData(1, 1, 2)]
    [InlineData(1, 2, 2)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 2, 0)]
    public async Task ValidateAndAddCourse_ShouldNotPassValidation(int course1, int course2, int course3)
    {
        // Arrange
        var lessons = new List<LessonUpsertDto>
        {
            SampleLessonDto(course1), SampleLessonDto(course2), SampleLessonDto(course3)
        };

        var course = SampleCourseDto(lessons);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Add(It.IsAny<Course>()));

        // Act
        var result = await _courseService.ValidateAndAddCourse(1, course);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1, 3, 2)]
    [InlineData(2, 1, 3)]
    [InlineData(2, 3, 1)]
    [InlineData(3, 1, 2)]
    [InlineData(3, 2, 1)]
    public async Task ValidateAndUpdateCourse_ShouldPassValidation(int course1, int course2, int course3)
    {
        // Arrange
        var lessons = new List<LessonUpsertDto>
        {
            SampleLessonDto(course1), SampleLessonDto(course2), SampleLessonDto(course3)
        };

        var course = SampleCourse();
        var courseDto = SampleCourseDto(lessons);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Update(course));

        // Act
        var result = await _courseService.ValidateAndUpdateCourse(course, courseDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(1, 3, 4)]
    [InlineData(1, 1, 2)]
    [InlineData(1, 2, 2)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 2, 0)]
    public async Task ValidateAndUpdateCourse_ShouldNotPassValidation(int course1, int course2, int course3)
    {
        // Arrange
        var lessons = new List<LessonUpsertDto>
        {
            SampleLessonDto(course1), SampleLessonDto(course2), SampleLessonDto(course3)
        };

        var course = SampleCourse();
        var courseDto = SampleCourseDto(lessons);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Update(course));

        // Act
        var result = await _courseService.ValidateAndUpdateCourse(course, courseDto);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task ValidateAndAddLesson_ShouldPassValidation(int newLessonNumber)
    {
        // Arrange
        var course = SampleCourse();
        var lesson = SampleLessonDto(newLessonNumber);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Add(course));

        // Act
        var result = await _courseService.ValidateAndAddLesson(course, lesson);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(OneToN(course.Lessons));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public async Task ValidateAndAddLesson_ShouldNotPassValidation(int newLessonNumber)
    {
        // Arrange
        var course = SampleCourse();
        var lesson = SampleLessonDto(newLessonNumber);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Add(course));

        // Act
        var result = await _courseService.ValidateAndAddLesson(course, lesson);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 1)]
    [InlineData(3, 2)]
    [InlineData(3, 3)]
    public async Task ValidateAndUpdateLesson_ShouldPassValidation(int lessonId, int newLessonNumber)
    {
        // Arrange
        var course = SampleCourse();
        var lesson = SampleLessonDto(newLessonNumber);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Add(course));

        // Act
        var result = await _courseService.ValidateAndUpdateLesson(course, lessonId, lesson);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(OneToN(course.Lessons));
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, 4)]
    [InlineData(2, 0)]
    [InlineData(2, 4)]
    [InlineData(3, 0)]
    [InlineData(3, 4)]
    public async Task ValidateAndUpdateLesson_ShouldNotPassValidation(int lessonId, int newLessonNumber)
    {
        // Arrange
        var course = SampleCourse();
        var lesson = SampleLessonDto(newLessonNumber);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Courses.Add(course));

        // Act
        var result = await _courseService.ValidateAndUpdateLesson(course, lessonId, lesson);

        // Assert
        Assert.False(result.IsSuccess);
    }

    private static bool OneToN(IEnumerable<Lesson> lessons)
    {
        var sortedNumbers = lessons
            .Select(lesson => lesson.LessonNumber)
            .Order()
            .ToArray();

        return !sortedNumbers.Where((t, i) => i + 1 != t).Any();
    }
}