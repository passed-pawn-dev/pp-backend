using PassedPawn.Models.DTOs.Course.Example.Move.Arrow;
using PassedPawn.Models.DTOs.Course.Example.Move.Highlight;

namespace PassedPawn.Models.DTOs.Course.Example.Move;

public class CourseExampleMoveUpsertDto
{
    public required string AlgebraicNotation { get; set; }
    public string? Description { get; set; }
    public IEnumerable<CourseExampleMoveArrowUpsertDto> Arrows { get; init; } = [];
    public IEnumerable<CourseExampleMoveHighlightUpsertDto> Highlights { get; init; } = [];
}
