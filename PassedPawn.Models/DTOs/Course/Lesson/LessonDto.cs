using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Exercise;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.Models.DTOs.Course.Lesson;

public class LessonDto
{
    public int Id { get; init; }
    public int LessonNumber { get; init; }
    public CourseVideoDto? Video { get; init; }
    public IEnumerable<CourseExerciseDto> Exercises { get; init; } = [];
    public IEnumerable<CourseExampleDto> Examples { get; init; } = [];
    public IEnumerable<CourseVideoDto> Videos { get; init; } = [];
}