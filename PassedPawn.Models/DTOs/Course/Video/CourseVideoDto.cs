namespace PassedPawn.Models.DTOs.Course.Video;

public class CourseVideoDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public int Order { get; init; }
    public required string VideoPublicId{ get; init; }
    public string? TemporaryVideoDownloadUrl { get; set; }
}
