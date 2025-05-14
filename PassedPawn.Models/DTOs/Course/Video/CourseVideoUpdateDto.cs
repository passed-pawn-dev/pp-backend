using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PassedPawn.Models.Validators;

namespace PassedPawn.Models.DTOs.Course.Video;

public class CourseVideoUpdateDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public int? Order { get; set; }
    [Required, VideoFile]
    public IFormFile? Video { get; init; }
}