using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Exercise;
using PassedPawn.Models.DTOs.Course.Quiz;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.Models.DTOs.Course.Lesson;

public class LessonDto
{
    public int Id { get; init; }
    public bool Preview { get; init; }
    public int LessonNumber { get; init; }
    public IEnumerable<CourseExerciseDto> Exercises { get; init; } = [];
    public IEnumerable<CourseExampleDto> Examples { get; init; } = [];
    public IEnumerable<CourseVideoDto> Videos { get; init; } = [];
    public IEnumerable<CourseQuizDto> Quizzes { get; init; } = [];
}
