using System.ComponentModel.DataAnnotations;
using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.Course.Example.Move.Arrow;

public class CourseExampleMoveArrowUpsertDto
{
    [Range(0, 63)] public int Source { get; init; }
    [Range(0, 63)] public int Destination { get; init; }
    public Severity Severity { get; init; }
}