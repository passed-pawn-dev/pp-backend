using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.User.Coach;

public class CoachPfpDto
{
    [Required, Url] public string PhotoUrl { get; init; } = "";
    [Required] public string PhotoPublicId { get; init; } = "";
}