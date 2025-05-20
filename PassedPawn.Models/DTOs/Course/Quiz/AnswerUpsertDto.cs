using PassedPawn.Models.Validators;

namespace PassedPawn.Models.DTOs.Course.Quiz;

public class AnswerUpsertDto
{
    public required string Text { get; init; }
    [FenValidation] public string? NewPosition { get; init; }
    public string? LastMove { get; init; }
}