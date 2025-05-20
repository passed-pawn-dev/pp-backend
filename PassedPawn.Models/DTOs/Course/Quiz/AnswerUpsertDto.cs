namespace PassedPawn.Models.DTOs.Course.Quiz;

public class AnswerUpsertDto
{
    public required string Text { get; init; }
    public string? NewPosition { get; init; }
    public string? LastMove { get; init; }
}