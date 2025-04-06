namespace PassedPawn.Models.DTOs.Course.Quiz;

public class CourseQuizUpsertDto
{
    public required string Question { get; set; }
    public ICollection<AnswerDto> Answers { get; set; } = [];
    public int Number { get; set; }
    public string? Hint { get; set; }
    public string? Position { get; set; }
    public string? Explanation  { get; set; }
}