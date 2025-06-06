namespace PassedPawn.Models.DTOs.Course;

public class BoughtCourseDetailsDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? ThumbnailUrl { get; init; }
    public GivenReviewDto? GivenReview { get; set; }
    public IEnumerable<BoughtCourseDetailsLessonDto> Lessons { get; init; } = [];
}

public class GivenReviewDto
{
    public int Id { get; init; }
    public decimal Value { get; init; }
    public string? Content { get; init; }
}

public class BoughtCourseDetailsLessonDto
{
    public int Id { get; init; }
    public int LessonNumber { get; init; }
    public required string Title { get; init; }
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Quizzes { get; init; } = [];
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Puzzles { get; init; } = [];
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Examples { get; init; } = [];
    public IEnumerable<BoughtCourseDetailsLessonElementSlimDto> Videos { get; init; } = [];
}

public class BoughtCourseDetailsLessonElementSlimDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public int Order { get; init; }
    public bool Completed { get; init; }
}
