using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IMapper mapper) : IUnitOfWork
{
    public ICoachRepository Coaches { get; } = new CoachRepository(dbContext, mapper);
    public IStudentRepository Students { get; } = new StudentRepository(dbContext, mapper);
    public IRepositoryBase<Nationality> Nationalities { get; } = new RepositoryBase<Nationality>(dbContext, mapper);
    public ICourseRepository Courses { get; } = new CourseRepository(dbContext, mapper);
    public ILessonRepository Lessons { get; } = new LessonRepository(dbContext, mapper);
    public IRepositoryBase<CourseReview> CourseReviews { get; } = new RepositoryBase<CourseReview>(dbContext, mapper);
    public ICourseExerciseRepository Puzzles { get; } = new CourseExerciseRepository(dbContext, mapper);
    public IRepositoryBase<CourseExample> Examples { get; } = new RepositoryBase<CourseExample>(dbContext, mapper);
    public IRepositoryBase<CourseQuiz> Quizzes { get; } = new RepositoryBase<CourseQuiz>(dbContext, mapper);


    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}