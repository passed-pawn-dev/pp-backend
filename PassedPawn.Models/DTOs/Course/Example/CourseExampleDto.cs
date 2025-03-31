using PassedPawn.Models.DTOs.Course.Example.Move;

namespace PassedPawn.Models.DTOs.Course.Example;

public class CourseExampleDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public string? InitialDescription { get; init; }
    public required string InitialFen { get; init; }
    public int Order { get; init; }

    public ICollection<CourseExampleMoveDto> Moves { get; init; } = [];
}