namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class CourseExample : CourseElement, IEntity
{
    public string? InitialDescription { get; set; }
    public ICollection<CourseExampleMove> Moves { get; init; } = [];
}
