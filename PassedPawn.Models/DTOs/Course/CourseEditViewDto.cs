using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course;

public class CourseEditViewDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public CourseStatus Status { get; init; }
    public float Price { get; init; }
    public int? EloRangeStart { get; init; }
    public int? EloRangeEnd { get; init; }
    public string? ThumbnailUrl { get; init; }
}