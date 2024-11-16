using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Exercise;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.Models.DTOs.Course.Lesson;

public class LessonUpsertDto
{
    public int LessonNumber { get; init; }
    public VideoUpsertDto? Video { get; init; }
    public IEnumerable<CourseExerciseUpsertDto> Exercises { get; init; } = [];
    public IEnumerable<CourseExampleUpsertDto> Examples { get; init; } = [];
}