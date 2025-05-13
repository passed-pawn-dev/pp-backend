using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.DataAccess.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IMapper mapper) : IUnitOfWork
{
    public ICoachRepository Coaches { get; } = new CoachRepository(dbContext, mapper);
    public IRepositoryBase<Photo> Photos { get; } = new RepositoryBase<Photo>(dbContext, mapper);
    public IStudentRepository Students { get; } = new StudentRepository(dbContext, mapper);
    public IRepositoryBase<Nationality> Nationalities { get; } = new RepositoryBase<Nationality>(dbContext, mapper);
    public ICourseRepository Courses { get; } = new CourseRepository(dbContext, mapper);
    public ILessonRepository Lessons { get; } = new LessonRepository(dbContext, mapper);
    public IRepositoryBase<CourseReview> CourseReviews { get; } = new RepositoryBase<CourseReview>(dbContext, mapper);
    public ICoursePuzzleRepository Puzzles { get; } = new CoursePuzzleRepository(dbContext, mapper);
    public ICourseExampleRepository Examples { get; } = new CourseExampleRepository(dbContext, mapper);
    public ICourseQuizRepository Quizzes { get; } = new CourseQuizRepository(dbContext, mapper);
    public ICourseVideoRepository Videos { get; } = new CourseVideoRepository(dbContext, mapper);


    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}