using Microsoft.AspNetCore.Http;

namespace PassedPawn.Models.DTOs.Course.Video;

public class CourseVideoUpsertDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public int Order { get; init; }
    public required IFormFile Video { get; init; }
}