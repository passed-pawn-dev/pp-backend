using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class StudentRepository(ApplicationDbContext dbContext, IMapper mapper) : 
    RepositoryBase<Student>(dbContext, mapper), IStudentRepository
{
    public async Task<Student?> GetUserByEmail(string email)
    {
        return await DbSet
            .Where(student => student.Email == email)
            .SingleOrDefaultAsync();
    }
}