namespace PassedPawn.Models.DTOs.Course;

public class NonBoughtCourseDetailsDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public int EnrolledStudentsCount { get; init; }
    public required NonBoughtCourseDetailsCoachDto Coach { get; init; }
    public int PuzzleCount { get; init; }
    public int VideoCount { get; init; }
    public int QuizCount { get; init; }
    public int ExampleCount { get; init; }
    public string Language { get; init; } = "English";
    public int? EloRangeStart { get; init; }
    public int? EloRangeEnd { get; init; }
    public int TotalVideoCount { get; init; }
    public int ReviewCount { get; init; }
    public decimal AverageScore { get; init; }
    public float Price { get; init; }
    public string? ThumbnailUrl { get; init; }
    public bool IsBought { get; set; }
    public IEnumerable<NonBoughtCourseDetailsLessonDto> Lessons { get; init; } = [];
}

public class NonBoughtCourseDetailsCoachDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string ChessTitle { get; init; }
    public int CreatedCoursesCount { get; init; }
    public required string Description { get; init; }
    public string? PictureUrl { get; init; }
}

public class NonBoughtCourseDetailsLessonDto
{
    public int Id { get; init; }
    public int LessonNumber { get; init; }
    public required string Title { get; init; }
    public bool Preview { get; init; }
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Quizzes { get; init; } = [];
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Puzzles { get; init; } = [];
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Examples { get; init; } = [];
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Videos { get; init; } = [];
}

public class NonBoughtCourseDetailsLessonElementSlimDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public int Order { get; init; }
    public bool Completed { get; init; }
}
