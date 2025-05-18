using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PassedPawn.Models.Validators;

namespace PassedPawn.Models.DTOs.Course;

public class CourseThumbnailDto
{
    [Required, ImageFile]
    public IFormFile? Thumbnail { get; init; }
}