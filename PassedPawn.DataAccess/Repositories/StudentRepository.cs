using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;

namespace PassedPawn.DataAccess.Repositories;

public class StudentRepository(ApplicationDbContext dbContext, IMapper mapper) : 
    RepositoryBase<Student>(dbContext, mapper), IStudentRepository
{
    public async Task<int?> GetIdByEmail(string email)
    {
        return await DbSet
            .Where(student => student.Email == email)
            .Select(user => user.Id)
            .SingleOrDefaultAsync();
    }

    public async Task<Student?> GetUserByEmail(string email)
    {
        return await DbSet
            .Where(student => student.Email == email)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<BoughtCourseDto>> GetStudentCoursesWhere(int userId, string? name)
    {
        var query = DbContext
            .Set<Course>()
            .Include(course => course.Students)
            .Where(course => course.Students.Any(student => student.Id == userId));

        if (name is not null)
            query = query.Where(course => course.Title.ToLower().Contains(name.ToLower()));
        
        return await query
            .ProjectTo<BoughtCourseDto>(MapperConfiguration)
            .ToListAsync();
    }

    public async Task<BoughtCourseDetailsDto?> GetStudentCourse(int userId, int courseId)
    {
        return await DbContext
            .Set<Course>()
            .Where(course => course.Id == courseId)
            .Include(course => course.Students)
            .Where(course => course.Students.Any(student => student.Id == userId))
            .ProjectTo<BoughtCourseDetailsDto>(MapperConfiguration)
            .SingleOrDefaultAsync();
    }

    public async Task<bool> IsCourseBought(int userId, int courseId)
    {
        return await DbSet
            .Where(student => student.Id == userId)
            .Include(student => student.Courses)
            .AnyAsync(student => student.Courses.Any(course => course.Id == courseId));
    }
}