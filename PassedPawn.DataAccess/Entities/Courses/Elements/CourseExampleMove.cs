namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseExampleMove : IEntity
{
    public int Id { get; set; }
    
    public required string Fen { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public ICollection<CourseExampleMoveArrow> Arrows { get; init; } = [];
    public ICollection<CourseExampleMoveHighlight> Highlights { get; init; } = [];
    
    public int ExampleId { get; init; }
    public CourseExample? Example { get; init; }
}