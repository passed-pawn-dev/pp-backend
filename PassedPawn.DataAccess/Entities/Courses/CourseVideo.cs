namespace PassedPawn.DataAccess.Entities.Courses;

public class CourseVideo : IEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int Id { get; set; }
}