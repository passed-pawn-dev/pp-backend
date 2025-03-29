using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course.Example.Move.Arrow;

public class CourseExampleMoveArrowUpsertDto
{
    public required string Source { get; init; }
    public required string Destination { get; init; }
    public Severity Severity { get; init; }
}