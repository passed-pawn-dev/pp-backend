using AutoMapper;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Quiz;

namespace PassedPawn.BusinessLogic.Services;

public class CourseQuizService(IUnitOfWork unitOfWork, IMapper mapper) : CourseElementService, ICourseQuizService
{
    public async Task<ServiceResult<CourseQuizDto>> ValidateAndAddQuiz(Lesson lesson, CourseQuizUpsertDto upsertDto)
    {
        var quiz = mapper.Map<CourseQuiz>(upsertDto);
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (quiz.Order > highestOrderNumber + 1 || quiz.Order < 1)
            return ServiceResult<CourseQuizDto>.Failure([
                $"New quiz has wrong order. Maximum of {highestOrderNumber + 1} expected"
            ]);

        MoveOrderOnAdd(lesson, quiz.Order);
        lesson.Quizzes.Add(quiz);
        unitOfWork.Lessons.Update(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseQuizDto>.Success(mapper.Map<CourseQuizDto>(quiz));
    }

    public async Task<ServiceResult<CourseQuizDto>> ValidateAndUpdateQuiz(Lesson lesson, int exampleId, 
        CourseQuizUpsertDto upsertDto)
    {
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (upsertDto.Order > highestOrderNumber || upsertDto.Order < 1)
            return ServiceResult<CourseQuizDto>.Failure([
                $"New quiz has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        var quiz = lesson.Quizzes.Single(quiz => quiz.Id == exampleId);
        MoveOrderOnUpdate(lesson, quiz.Order, upsertDto.Order);
        mapper.Map(upsertDto, quiz);
        unitOfWork.Quizzes.Update(quiz);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseQuizDto>.Success(mapper.Map<CourseQuizDto>(quiz));
    }
}