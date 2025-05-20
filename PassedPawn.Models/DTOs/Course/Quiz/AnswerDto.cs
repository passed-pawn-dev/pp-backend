namespace PassedPawn.Models.DTOs.Course.Quiz;

public class AnswerDto
{
    public int Id { get; init; }
    public required string Text { get; init; }
    public string? NewPosition { get; init; }
    public string? LastMove { get; init; }
}