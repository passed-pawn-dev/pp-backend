namespace PassedPawn.Models.DTOs.Course.Quiz;

public class AnswerUpsertDto
{
    public required string Text { get; set; }
    public string? LastMove;
}