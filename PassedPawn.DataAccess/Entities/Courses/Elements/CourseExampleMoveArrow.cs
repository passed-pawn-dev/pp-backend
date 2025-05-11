using PassedPawn.Models.Enums;

namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseExampleMoveArrow : IEntity
{
    public int Id { get; set; }
    public int Source { get; set; }
    public int Destination { get; set; }
    public Severity Severity { get; set; }
    
    public int ExampleMoveId { get; init; }
    public CourseExampleMove? ExampleMove { get; init; }
}