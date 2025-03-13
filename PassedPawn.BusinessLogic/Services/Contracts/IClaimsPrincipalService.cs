using System.Security.Claims;
using PassedPawn.DataAccess.Entities;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IClaimsPrincipalService
{
    Task<int> GetStudentId(ClaimsPrincipal principal);
    Task<int?> GetStudentIdOptional(ClaimsPrincipal principal);
    Task<Student> GetStudent(ClaimsPrincipal principal);
    Task<int> GetCoachId(ClaimsPrincipal principal);
    bool IsLoggedInAsStudent(ClaimsPrincipal principal);
}