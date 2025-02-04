using PassedPawn.Models.DTOs.Course.Review;

namespace PassedPawn.Models.DTOs.Course;

public class NonUserCourse
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public float Price { get; init; }
    public int LessonNumber { get; init; }
    public int StudentNumber { get; init; }
    public IEnumerable<CourseReviewDto> Reviews { get; init; } = [];
}