using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course;

public class CoachProfileDto
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public int Elo { get; init; }
    public ChessTitle? ChessTitle { get; init; }
    public string? Nationality { get; init; }
    public string? DetailedDescription { get; init; }
    public string? ShortDescription { get; init; }
    public required string PhotoUrl { get; init; }
    public IEnumerable<CourseDto> Courses { get; init; } = [];
}
