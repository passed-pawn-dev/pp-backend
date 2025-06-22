using System.ComponentModel.DataAnnotations;
using PassedPawn.Models.Validators;

namespace PassedPawn.Models.DTOs.Course.Quiz;

public class CourseQuizUpsertDto
{
    [Required]
    public string Question { get; init; } = string.Empty;
    public ICollection<AnswerUpsertDto> Answers { get; init; } = [];
    public string? Hint { get; init; }
    public int Solution { get; init; }
    [FenValidation]
    public string? Fen { get; init; }
    public string? Explanation  { get; init; }
    public int? Order { get; set; }
    [Required] 
    public string Title { get; set; } = string.Empty;
}