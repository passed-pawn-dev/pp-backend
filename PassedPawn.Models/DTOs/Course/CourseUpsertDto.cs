using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Photo;
using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course;

public class CourseUpsertDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public float Price { get; init; }
    public int? EloRangeStart { get; init; }
    public int? EloRangeEnd { get; init; }
    public CourseStatus Status { get; init; } = CourseStatus.InDraft;
    public PhotoUpsertDto? Thumbnail { get; init; }
    public IEnumerable<LessonUpsertDto> Lessons { get; init; } = [];
}