using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Course;

public class CourseThumbnailDto
{
    [Required, Url] public string PhotoUrl { get; init; } = "";
    [Required] public string PhotoPublicId { get; init; } = "";
}