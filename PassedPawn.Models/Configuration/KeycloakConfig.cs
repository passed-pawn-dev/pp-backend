using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.Configuration;

public class KeycloakConfig
{
    public string? BaseUrl { get; set; }
    public string? Realm { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? GrandType { get; set; }
}