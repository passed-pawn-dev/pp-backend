using System.ComponentModel.DataAnnotations;
using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course.Example.Move.Highlight;

public class CourseExampleMoveHighlightUpsertDto
{
    [Range(0, 63)] public int Position { get; init; }
    public Severity Severity { get; init; }
}