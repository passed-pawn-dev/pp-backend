using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;
using PassedPawn.Models.DTOs.Photo;

namespace PassedPawn.Models.DTOs.Course;

public class CourseDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public float Price { get; init; }
    public PhotoDto? Thumbnail { get; init; }
    public IEnumerable<LessonDto> Lessons { get; init; } = [];
    public IEnumerable<CourseReviewDto> Reviews { get; init; } = [];
}