namespace PassedPawn.DataAccess.Entities.Courses;

public class Course : IEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }

    public float Price { get; set; }

    public int? ThumbnailId { get; set; }
    public Photo? Thumbnail { get; set; }
    
    public int CoachId { get; set; }
    public Coach? Coach { get; set; }

    public ICollection<Student> Students { get; init; } = [];
    public ICollection<Lesson> Lessons { get; init; } = [];
    public ICollection<CourseReview> Reviews { get; init; } = [];
    public int Id { get; set; }
}