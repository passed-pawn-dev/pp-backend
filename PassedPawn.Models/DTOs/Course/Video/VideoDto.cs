namespace PassedPawn.Models.DTOs.Course.Video;

public class VideoDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}