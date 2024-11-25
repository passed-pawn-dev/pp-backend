namespace PassedPawn.Models.DTOs.Course.Exercise;

public class CourseExerciseUpsertDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string Pgn { get; init; }
    public int Order { get; init; }
}