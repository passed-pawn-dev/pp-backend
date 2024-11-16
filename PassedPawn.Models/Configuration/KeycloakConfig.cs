using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.Configuration;

public class KeycloakConfig
{
    [Required]
    public string? BaseUrl { get; set; }
    
    [Required]
    public string? Realm { get; set; }
    
    [Required]
    public string? ClientId { get; set; }
    
    [Required]
    public string? ClientSecret { get; set; }
    
    [Required]
    public string? GrandType { get; set; }
}