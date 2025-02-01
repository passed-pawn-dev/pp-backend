using PassedPawn.Models.DTOs.Photo;
using PassedPawn.Models.Enums;

namespace PassedPawn.Models.DTOs.User;

public abstract class UserDto
{
    public int Id { get; init; }

    public string Username { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string? PhoneNumber { get; init; }

    public PhotoDto? Photo { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public int Elo { get; init; }

    public string? ChessTitle { get; init; }

    public int NationalityId { get; init; }
}