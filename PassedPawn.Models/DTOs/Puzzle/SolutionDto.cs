using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Puzzle;

public class SolutionDto
{
    [Required]
    [RegularExpression(@"^[^,\s][^,\s]*(,[^,\s]+)*[^,\s]$")]
    public string Solution { get; init; } = string.Empty;
}