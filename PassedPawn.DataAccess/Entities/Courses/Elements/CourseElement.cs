namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public abstract class CourseElement
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Order { get; set; }
}
