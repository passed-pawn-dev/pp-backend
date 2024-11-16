using System.ComponentModel.DataAnnotations;
using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Entities;

public class CourseReview : IEntity
{
    public int Id { get; set; }
    [Range(1, 5)] public int Value { get; set; }
    public string? Content { get; set; }
    
    public int CourseId { get; init; }
    public Course? Course { get; init; }
    
    // TODO: Add StudentId
}
