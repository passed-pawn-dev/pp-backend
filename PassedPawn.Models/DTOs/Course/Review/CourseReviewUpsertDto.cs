using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Course.Review;

public class CourseReviewUpsertDto
{
    [Range(1, 5)] public int Value { get; init; }
    public string? Content { get; init; }
}