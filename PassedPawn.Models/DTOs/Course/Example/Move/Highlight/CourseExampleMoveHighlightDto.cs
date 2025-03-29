using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course.Example.Move.Highlight;

public class CourseExampleMoveHighlightDto
{
    public int Id { get; init; }
    public required string Position { get; init; }
    public Severity Severity { get; init; }
}