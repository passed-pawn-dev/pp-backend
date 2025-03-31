using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Exercise;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICourseExerciseService
{
    public Task<ServiceResult<CourseExerciseDto>> ValidateAndAddExercise(Lesson lesson, CourseExerciseUpsertDto upsertDto);

    public Task<ServiceResult<CourseExerciseDto>> ValidateAndUpdateExercise(Lesson lesson, int exampleId,
        CourseExerciseUpsertDto upsertDto);
}
