namespace PassedPawn.DataAccess.Entities.Courses;

public class CourseExampleMove : IEntity
{
    public int Id { get; set; }
    
    public required string AlgebraicNotation { get; set; }
    public string? Description { get; set; }
    public ICollection<CourseExampleMoveArrow> Arrows { get; init; } = [];
    public ICollection<CourseExampleMoveHighlight> Highlights { get; init; } = [];
    
    public int ExampleId { get; init; }
    public CourseExampleMove? Example { get; init; }
}