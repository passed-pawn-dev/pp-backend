using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories;

public class CoachRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<Coach>(dbContext, mapper), ICoachRepository
{
    public async Task<IEnumerable<CoachCourseDto>> GetCoachCourses(int coachId)
    {
        return await DbContext
            .Set<Course>()
            .Where(course => course.CoachId == coachId)
            .ProjectTo<CoachCourseDto>(MapperConfiguration)
            .ToListAsync();
    }

    public async Task<T?> GetCoachCourse<T>(int courseId, int coachId)
    {
        return await DbContext
            .Set<Course>()
            .Where(course => course.CoachId == coachId && course.Id == courseId)
            .ProjectTo<T>(MapperConfiguration)
            .SingleOrDefaultAsync();
    }

    public async Task<int?> GetUserIdByEmail(string email)
    {
        return await DbSet
            .Where(coach => coach.Email == email)
            .Select(coach => coach.Id)
            .SingleOrDefaultAsync();
    }

    public async Task<Coach?> GetWithPhotoById(int id)
    {
        return await DbSet
            .Where(coach => coach.Id == id)
            .Include(coach => coach.Photo)
            .SingleOrDefaultAsync();
    }

    public async Task<bool> EmailExists(string email)
    {
        return await DbSet
            .AnyAsync(coach => coach.Email == email);
    }
}