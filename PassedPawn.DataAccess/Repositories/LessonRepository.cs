using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.DataAccess.Repositories;

public class LessonRepository(ApplicationDbContext dbContext, IMapper mapper)
    : RepositoryBase<Lesson>(dbContext, mapper), ILessonRepository
{
    public async Task<IEnumerable<BoughtCourseDetailsLessonDto>> GetUserLessons(int userId, int courseId)
    {
        return await DbSet
            .Include(lesson => lesson.Course)
            .ThenInclude(course => course!.Students)
            .Where(lesson =>
                lesson.CourseId == courseId && lesson.Course!.Students.Any(student => student.Id == userId))
            .ProjectTo<BoughtCourseDetailsLessonDto>(MapperConfiguration)
            .ToListAsync();
    }

    public async Task<Lesson?> GetWithElementsAndCoachById(int lessonId)
    {
        return await DbSet
            .Where(lesson => lesson.Id == lessonId)
            .Include(lesson => lesson.Examples)
            .Include(lesson => lesson.Puzzles)
            .Include(lesson => lesson.Videos)
            .Include(lesson => lesson.Course)
            .Include(lesson => lesson.Quizzes)
            .SingleOrDefaultAsync();

    }

    public async Task<Lesson?> GetByExampleId(int exampleId)
    {
        return await DbSet
            .Include(lesson => lesson.Examples)
            .Where(lesson => lesson.Examples.Any(example => example.Id == exampleId))
            .Include(lesson => lesson.Puzzles)
            .Include(lesson => lesson.Videos)
            .Include(lesson => lesson.Course)
            .Include(lesson => lesson.Quizzes)
            .SingleOrDefaultAsync();

    }

    public async Task<Lesson?> GetByPuzzleId(int puzzleId)
    {
        return await DbSet
            .Include(lesson => lesson.Puzzles)
            .Where(lesson => lesson.Puzzles.Any(puzzle => puzzle.Id == puzzleId))
            .Include(lesson => lesson.Examples)
            .Include(lesson => lesson.Videos)
            .Include(lesson => lesson.Course)
            .Include(lesson => lesson.Quizzes)
            .SingleOrDefaultAsync();
    }

    public async Task<Lesson?> GetByVideoId(int videoId)
    {
        return await DbSet
            .Include(lesson => lesson.Videos)
            .Where(lesson => lesson.Videos.Any(video => video.Id == videoId))
            .Include(lesson => lesson.Puzzles)
            .Include(lesson => lesson.Examples)
            .Include(lesson => lesson.Course)
            .Include(lesson => lesson.Quizzes)
            .SingleOrDefaultAsync();
    }

    public async Task<Lesson?> GetByQuizId(int quizId)
    {
        return await DbSet
            .Include(lesson => lesson.Quizzes)
            .ThenInclude(quiz => quiz.Answers)
            .Where(lesson => lesson.Quizzes.Any(quiz => quiz.Id == quizId))
            .Include(lesson => lesson.Puzzles)
            .Include(lesson => lesson.Examples)
            .Include(lesson => lesson.Course)
            .Include(lesson => lesson.Videos)
            .SingleOrDefaultAsync();
    }
}
