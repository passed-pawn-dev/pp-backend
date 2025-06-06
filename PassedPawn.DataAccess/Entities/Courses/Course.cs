using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.Enums;

namespace PassedPawn.DataAccess.Entities.Courses;

public class Course : IEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.InDraft;
    public float Price { get; set; }
    
    public int? EloRangeStart { get; set; }
    public int? EloRangeEnd { get; set; }

    public int? ThumbnailId { get; set; }
    public Photo? Thumbnail { get; set; }
    
    public int CoachId { get; set; }
    public Coach? Coach { get; set; }
    public DateTime ReleaseDate { get; set; }

    public ICollection<Student> Students { get; init; } = [];
    public ICollection<Lesson> Lessons { get; init; } = [];
    public ICollection<CourseReview> Reviews { get; init; } = [];
    public ICollection<CourseExample> Examples { get; init; } = [];
    public int Id { get; set; }
}