using System.ComponentModel.DataAnnotations;
using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Puzzle;
using PassedPawn.Models.DTOs.Course.Quiz;
using PassedPawn.Models.DTOs.Course.Video;

namespace PassedPawn.Models.DTOs.Course.Lesson;

public class LessonUpsertDto
{
    public int LessonNumber { get; init; }
    [Required] public string Title { get; init; } = "";
    public bool Preview { get; init; } = false;
}