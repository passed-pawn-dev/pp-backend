namespace PassedPawn.Models.DTOs.Course.Video;

public class CourseVideoUpdateDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public int? Order { get; set; }
    public string? VideoUrl { get; init; }
    public string? VideoPublicId { get; init; }
}