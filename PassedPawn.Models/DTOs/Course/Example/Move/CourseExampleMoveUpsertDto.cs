using PassedPawn.Models.DTOs.Course.Example.Move.Arrow;
using PassedPawn.Models.DTOs.Course.Example.Move.Highlight;

namespace PassedPawn.Models.DTOs.Course.Example.Move;

public class CourseExampleMoveUpsertDto
{
    public required string Fen { get; init; }
    public int Order { get; init; }
    public string? Description { get; init; }
    public IEnumerable<CourseExampleMoveArrowUpsertDto> Arrows { get; init; } = [];
    public IEnumerable<CourseExampleMoveHighlightUpsertDto> Highlights { get; init; } = [];
}
