namespace PassedPawn.DataAccess.Entities.Courses;

public class CourseExample : IEntity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? InitialDescription { get; set; }
    public required string InitialFen { get; set; }
    public int Order { get; set; }

    public ICollection<CourseExampleMove> Moves { get; init; } = [];

    public int LessonId { get; init; }
    public Lesson? Lesson { get; init; }
}