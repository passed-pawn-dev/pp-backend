namespace PassedPawn.Models.DTOs;

public class CloudinarySecureUrl
{
    public required string Timestamp { get; init; }
    public required string Signature { get; init; }
    public required string ApiKey { get; init; }
    public required string CloudName { get; init; }
    public required string Folder { get; init; }
    public required string ResourceType { get; init; }
    public required string AccessType { get; init; }
    public required object AccessControl { get; init; }
    public required string Invalidate { get; init; }
}
