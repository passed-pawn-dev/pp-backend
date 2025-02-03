namespace PassedPawn.Models.DTOs.Course.Exercise;

public class CourseExerciseDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Fen { get; init; }
    public required string Solution { get; set; }
    public int Order { get; init; }
}