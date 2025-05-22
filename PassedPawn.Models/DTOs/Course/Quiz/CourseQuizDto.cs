namespace PassedPawn.Models.DTOs.Course.Quiz;

public class CourseQuizDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Question { get; init; }
    public ICollection<AnswerDto> Answers { get; init; } = [];
    public int Solution { get; init; }
    public string? Hint { get; init; }
    public string? Fen { get; init; }
    public string? Explanation  { get; init; }
}