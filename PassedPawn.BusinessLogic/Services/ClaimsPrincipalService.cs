using System.Security.Claims;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.BusinessLogic.Services;

public class ClaimsPrincipalService(IUnitOfWork unitOfWork) : IClaimsPrincipalService
{
    public async Task<int> GetStudentId(ClaimsPrincipal principal)
    {
        var email = GetUserEmail(principal);
        return await unitOfWork.Students.GetIdByEmail(email)
               ?? throw new Exception("Student does not exist in database");
    }

    public async Task<int?> GetStudentIdOptional(ClaimsPrincipal principal)
    {
        var email = GetUserEmailOptional(principal);

        if (email is null)
            return null;
        
        return await unitOfWork.Students.GetIdByEmail(email)
               ?? null;
    }

    public async Task<Student> GetStudent(ClaimsPrincipal principal)
    {
        var email = GetUserEmail(principal);
        return await unitOfWork.Students.GetUserByEmail(email)
               ?? throw new Exception("Student does not exist");
    }

    public async Task<int> GetCoachId(ClaimsPrincipal principal)
    {
        var email = GetUserEmail(principal);
        return await unitOfWork.Coaches.GetUserIdByEmail(email)
               ?? throw new Exception("Coach not found in database");
    }

    public bool IsLoggedInAsStudent(ClaimsPrincipal principal)
    {
        return principal.Identity is { IsAuthenticated: true } && principal.IsInRole("student");
    }

    public bool IsLoggedInAsCoach(ClaimsPrincipal principal)
    {
        return principal.Identity is { IsAuthenticated: true } && principal.IsInRole("coach");
    }

    private static string GetUserEmail(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims
            .First(x => x.Type == ClaimTypes.Email).Value;
    }
    
    private static string? GetUserEmailOptional(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
    }
}