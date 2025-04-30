using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PassedPawn.Models.DTOs.Course;

public class CourseThumbnailDto
{
    [Required]
    public IFormFile? Thumbnail { get; init; }
}