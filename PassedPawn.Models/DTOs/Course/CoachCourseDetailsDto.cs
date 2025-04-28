namespace PassedPawn.Models.DTOs.Course;

public class CoachCourseDetailsDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public double AverageScore { get; init; }
    public float Price { get; init; }
    public int EnrolledStudentsCount { get; init; }
    public string? ThumbnailUrl { get; init; }
    public IEnumerable<CoachCourseDetailsLessonDto> Lessons { get; init; } = [];
}

public class CoachCourseDetailsLessonDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public IEnumerable<CoachCourseDetailsLessonElementDto> Quizzes { get; init; } = [];
    public IEnumerable<CoachCourseDetailsLessonElementDto> Puzzles { get; init; } = [];
    public IEnumerable<CoachCourseDetailsLessonElementDto> Videos { get; init; } = [];
    public IEnumerable<CoachCourseDetailsLessonElementDto> Examples { get; init; } = [];
}

public class CoachCourseDetailsLessonElementDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public int Order { get; init; }
}
