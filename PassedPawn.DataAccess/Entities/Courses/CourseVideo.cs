namespace PassedPawn.DataAccess.Entities.Courses;

public class CourseVideo : IEntity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}