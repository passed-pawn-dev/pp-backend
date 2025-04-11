using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.Models.DTOs.Course;

public class CourseDetailsDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string CoachName { get; init; }
    public IEnumerable<LessonDto> Lessons { get; init; } = [];
}
