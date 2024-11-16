namespace PassedPawn.Models.DTOs.Course.Example;

public class CourseExampleUpsertDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string Pgn { get; init; }
    public int Order { get; init; }
}
