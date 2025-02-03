namespace PassedPawn.DataAccess.Entities.Courses;

public class CourseExercise : IEntity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Order { get; set; }
    public required string Fen { get; set; }
    public required string Solution { get; set; }

    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    
    public List<Student> Students { get; set; } = [];
}