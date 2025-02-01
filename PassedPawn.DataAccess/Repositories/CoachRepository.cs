using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class CoachRepository(ApplicationDbContext dbContext, IMapper mapper) : RepositoryBase<Coach>(dbContext, mapper), ICoachRepository
{
    public async Task<int?> GetUserIdByEmail(string email)
    {
        return await DbSet
            .Where(coach => coach.Email == email)
            .Select(coach => coach.Id)
            .SingleOrDefaultAsync();
    }
}