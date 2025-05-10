namespace PassedPawn.Models.DTOs.Course;

public class CourseDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public float Price { get; init; }
    public int? EloRageStart { get; init; }
    public int? EloRangeEnd { get; init; }
    public required string CoachName { get; init; }
    public double AverageScore { get; init; }
    public string? PictureUrl { get; init; }
    public bool IsBought { get; init; }
    public int EnrolledStudentsCount { get; init; }
}
