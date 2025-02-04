using System.Security.Claims;

namespace PassedPawn.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    ///     Gets the user id from the claims principal
    /// </summary>
    /// <param name="claimsPrincipal">User's claims principal</param>
    /// <exception cref="NullReferenceException">User not logged in, or id not present in claims</exception>
    /// <returns>User's id</returns>
    public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims
            .First(x => x.Type == ClaimTypes.Email).Value;
    }
    
    public static string? GetUserEmailOptional(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
    }
}