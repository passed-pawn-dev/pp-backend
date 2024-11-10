namespace PassedPawn.DataAccess.Entities.Courses;

public class Course : IEntity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    
    // TODO: Add coach Id
    
    public float Price { get; set; }
    
    public int ThumbnailId { get; set; }
    public Photo? Thumbnail { get; set; }

    public ICollection<Lesson> Lessons { get; init; } = [];
}
