using Microsoft.AspNetCore.Http;

namespace PassedPawn.Models.DTOs.Course.Video;

public class CourseVideoAddDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public int? Order { get; set; }
    public required IFormFile Video { get; init; }
}