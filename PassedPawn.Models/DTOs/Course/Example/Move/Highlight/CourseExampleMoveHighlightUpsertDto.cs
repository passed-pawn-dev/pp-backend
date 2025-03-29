using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course.Example.Move.Highlight;

public class CourseExampleMoveHighlightUpsertDto
{
    public required string Position { get; init; }
    public Severity Severity { get; init; }
}