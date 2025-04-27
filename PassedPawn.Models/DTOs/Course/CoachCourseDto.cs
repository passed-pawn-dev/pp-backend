namespace PassedPawn.Models.DTOs.Course;

public class CoachCourseDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public double AverageScore { get; init; }
    public float Price { get; init; }
    public int EloRangeStart { get; init; }
    public int EloRangeEnd { get; init; }
    public int LessonCount { get; init; }
    public int ElementCount { get; init; }
}
