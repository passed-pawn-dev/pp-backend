namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CoursePuzzle : CourseElement, IEntity
{
    public required string Description { get; set; }
    public required string Fen { get; set; }
    public required string Solution { get; set; }
    
    public List<Student> Students { get; set; } = [];
}