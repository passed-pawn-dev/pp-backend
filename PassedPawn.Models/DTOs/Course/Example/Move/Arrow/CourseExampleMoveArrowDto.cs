using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course.Example.Move.Arrow;

public class CourseExampleMoveArrowDto
{
    public int Id { get; init; }
    public int Source { get; init; }
    public int Destination { get; init; }
    public Severity Severity { get; init; }
}