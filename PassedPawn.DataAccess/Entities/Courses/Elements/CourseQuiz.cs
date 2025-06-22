namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseQuiz : CourseElement, IEntity
{
    public required string Question { get; set; }

    public ICollection<QuizAnswer> Answers { get; set; } = [];

    public int Solution { get; set; }
    
    public string? Hint { get; set; }
    
    public string? Fen { get; set; }
    
    public string? Explanation  { get; set; }
}
