using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PassedPawn.Models.DTOs.User.Coach;

public class CoachPfpDto
{
    [Required]
    public IFormFile? Pfp { get; init; }
}