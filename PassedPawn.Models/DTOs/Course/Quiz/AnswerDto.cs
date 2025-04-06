namespace PassedPawn.Models.DTOs.Course.Quiz;

public class AnswerDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    // ToDO give right type
    public object LastMove;
}