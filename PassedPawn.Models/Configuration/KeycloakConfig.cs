using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.Configuration;

public class KeycloakConfig
{
    [Required] public string BaseUrl { get; set; } = string.Empty;

    [Required] public string Realm { get; set; } = string.Empty;

    [Required] public string ClientId { get; set; } = string.Empty;

    [Required] public string ClientSecret { get; set; } = string.Empty;

    [Required] public string GrandType { get; set; } = string.Empty;
}