using System.Collections;

namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseQuiz : CourseElement, IEntity
{
    public int QuizId { get; set; }
    
    public required string Question { get; set; }

    public ICollection<QuizAnswer> Answers { get; set; } = [];

    public int Number { get; set; }
    
    public string? Hint { get; set; }
    
    // TODO make it same in all entities
    public string? Position { get; set; }
    
    public string? Explanation  { get; set; }
    
    public int LessonId { get; init; }
}
