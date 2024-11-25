namespace PassedPawn.DataAccess.Entities.Courses;

public class CourseExercise : IEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Pgn { get; set; }
    public int Order { get; set; }

    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    public int Id { get; set; }
}