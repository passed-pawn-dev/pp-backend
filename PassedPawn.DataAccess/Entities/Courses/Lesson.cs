namespace PassedPawn.DataAccess.Entities.Courses;

public class Lesson : IEntity
{
    public int Id { get; set; }
    public int LessonNumber { get; set; }
    
    public int VideoId { get; set; }
    public CourseVideo? Video { get; set; }

    public ICollection<CourseExercise> Exercises { get; init; } = [];
    public ICollection<CourseExample> Examples { get; init; } = [];
}