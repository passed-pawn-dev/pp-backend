namespace PassedPawn.Models.DTOs.Course.Quiz;

public class AnswerDto
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public string? LastMove;
}