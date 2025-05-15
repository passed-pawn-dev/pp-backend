using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course;

// TODO: Add progress field. Which data type should it be?
public class BoughtCourseDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public CourseStatus Status { get; init; }
    public int? EloRangeStart { get; init; }
    public int? EloRangeEnd { get; init; }
    public required string CoachName { get; init; }
    public string? ThumbnailUrl { get; init; }
}
