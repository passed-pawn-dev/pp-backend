using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Course.Exercise;

public class SolutionDto
{
    [Required] public required string Solution { get; init; }
}
