namespace PassedPawn.Models.DTOs.Course.Review;

public class CourseReviewDto
{
    public int Id { get; init; }
    public decimal Value { get; init; }
    public string? Content { get; init; }
    public required string Author { get; init; }
}