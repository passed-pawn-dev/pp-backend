namespace PassedPawn.Models.DTOs.Keycloak;

public class RoleDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Composite { get; set; }
    public bool ClientRole { get; set; }
    public string ContainerId { get; set; } = string.Empty;
    public Dictionary<string, object> Attributes { get; set; } = new();
}