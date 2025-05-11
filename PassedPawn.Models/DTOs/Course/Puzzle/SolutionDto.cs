using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Course.Puzzle;

public class SolutionDto
{
    [Required]
    [RegularExpression(@"^[^,\s][^,\s]*(,[^,\s]+)*[^,\s]$")]
    public required string Solution { get; init; }
}
