using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PassedPawn.Models.Validators;

namespace PassedPawn.Models.DTOs.User.Coach;

public class CoachPfpDto
{
    [Required, ImageFile]
    public IFormFile? Pfp { get; init; }
}