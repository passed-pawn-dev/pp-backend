using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Course.Exercise;

public class CourseExerciseUpsertDto
{
    [Required] public string Title { get; init; } = string.Empty;
    [Required] public string Description { get; init; } = string.Empty;
    [Required] public string Fen { get; init; } = string.Empty;
    public int Order { get; init; }
    
    [Required]
    [RegularExpression(@"^[^,\s][^,\s]*(,[^,\s]+)*[^,\s]$")]
    public string Solution { get; set; } = string.Empty;
}