using PassedPawn.Models.DTOs.Course.Example.Move;

namespace PassedPawn.Models.DTOs.Course.Example;

public class CourseExampleUpsertDto
{
    public required string Title { get; init; }
    public string? InitialDescription { get; init; }
    public int? Order { get; set; }

    public ICollection<CourseExampleMoveUpsertDto> Steps { get; init; } = [];
}