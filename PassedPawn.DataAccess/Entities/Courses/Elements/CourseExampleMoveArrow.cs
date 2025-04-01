using PassedPawn.Models.Enums;

namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseExampleMoveArrow : IEntity
{
    public int Id { get; set; }
    public required string Source { get; set; }
    public required string Destination { get; set; }
    public Severity Severity { get; set; }
    
    public int ExampleMoveId { get; init; }
    public CourseExampleMove? ExampleMove { get; init; }
}