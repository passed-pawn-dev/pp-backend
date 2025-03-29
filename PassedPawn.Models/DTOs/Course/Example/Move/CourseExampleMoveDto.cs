using PassedPawn.Models.DTOs.Course.Example.Move.Arrow;
using PassedPawn.Models.DTOs.Course.Example.Move.Highlight;

namespace PassedPawn.Models.DTOs.Course.Example.Move;

public class CourseExampleMoveDto
{
    public int Id { get; set; }
    
    public required string AlgebraicNotation { get; set; }
    public string? Description { get; set; }
    public IEnumerable<CourseExampleMoveArrowDto> Arrows { get; init; } = [];
    public IEnumerable<CourseExampleMoveHighlightDto> Highlights { get; init; } = [];
}