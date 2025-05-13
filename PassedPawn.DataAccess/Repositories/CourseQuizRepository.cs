using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Quiz;

namespace PassedPawn.DataAccess.Repositories;

public class CourseQuizRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<CourseQuiz>(dbContext, mapper), ICourseQuizRepository
{
    public async Task<CourseQuizDto?> GetOwnedOrInPreviewAsync(int quizId, int userId)
    {
        return await DbSet
            .Include(quiz => quiz.Lesson)
            .ThenInclude(lesson => lesson!.Course)
            .ThenInclude(course => course!.Students)
            .Where(quiz => quiz.Id == quizId &&
                             (quiz.Lesson!.Preview || quiz.Lesson.Course!.Students.Any(student => student.Id == userId)))
            .ProjectTo<CourseQuizDto>(MapperConfiguration)
            .SingleOrDefaultAsync();
    }
}