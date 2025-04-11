using System.Collections;

namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class QuizAnswer : IEntity
{
    public int Id { get; set; }
    
    public required string Text { get; set; }
    
    public string? LastMove { get; set; }
    
    public int QuizId { get; init; }
    
    public CourseQuiz? Quiz { get; init; }
}