namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseVideo : CourseElement, IEntity
{
    public string? Description { get; set; }
    
    public required string VideoPublicId { get; set; }
}
