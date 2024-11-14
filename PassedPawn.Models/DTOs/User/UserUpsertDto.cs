using System.ComponentModel.DataAnnotations;
using PassedPawn.Models.DTOs.Photo;
using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.User;

public class UserUpsertDto
{
    [Required]
    public string Username { get; init; } = string.Empty;
    
    [Required]
    public string FirstName { get; init; } = string.Empty;
    
    [Required]
    public string LastName { get; init; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
    
    [Required]
    public string Password { get; init; } = string.Empty;
    
    public string? PhoneNumber { get; init; }
    
    public PhotoUpsertDto? Photo { get; init; }
    
    [Required]
    public DateOnly DateOfBirth { get; init; }
    
    [Required]
    public int Elo { get; init; }
    
    public ChessTitle? ChessTitle { get; init; }

    [Required]
    public int NationalityId { get; init; }
}