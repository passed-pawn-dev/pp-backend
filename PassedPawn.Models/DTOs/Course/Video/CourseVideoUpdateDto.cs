using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Course.Video;

public class CourseVideoUpdateDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Order { get; set; }
    public required string VideoPublicId { get; init; }
}