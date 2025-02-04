namespace PassedPawn.Models.DTOs.Keycloak;

public class UserRegistrationDto
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public bool Enabled { get; } = true;
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    // public required Dictionary<string, IEnumerable<string>> ClientRoles { get; set; }
    public required List<CredentialDto> Credentials { get; init; }
}

public class CredentialDto
{
    public string Type { get; } = "password";
    public required string Value { get; set; }
    public bool Temporary { get; } = false;
}