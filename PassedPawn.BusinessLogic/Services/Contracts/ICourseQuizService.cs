using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Quiz;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICourseQuizService
{
    public Task<ServiceResult<CourseQuizDto>> ValidateAndAddQuiz(Lesson lesson, CourseQuizUpsertDto upsertDto);

    public Task<ServiceResult<CourseQuizDto>> ValidateAndUpdateQuiz(Lesson lesson, int exampleId,
        CourseQuizUpsertDto upsertDto);
}