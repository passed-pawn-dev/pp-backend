using PassedPawn.Models.Enums;

namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseExampleMoveHighlight : IEntity
{
    public int Id { get; set; }
    public int Position { get; set; }
    public Severity Severity { get; set; }
    
    public int ExampleMoveId { get; init; }
    public CourseExampleMove? ExampleMove { get; init; }
}