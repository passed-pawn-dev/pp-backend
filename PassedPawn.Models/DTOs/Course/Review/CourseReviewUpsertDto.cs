using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Course.Review;

public class CourseReviewUpsertDto
{
    [Range(1, 5)] public decimal Value { get; init; }
    public string? Content { get; init; }
}