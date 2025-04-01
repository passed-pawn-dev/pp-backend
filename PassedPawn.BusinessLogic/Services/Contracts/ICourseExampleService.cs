using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Example;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICourseExampleService
{
    public Task<ServiceResult<CourseExampleDto>> ValidateAndAddExample(Lesson lesson, CourseExampleUpsertDto upsertDto);

    public Task<ServiceResult<CourseExampleDto>> ValidateAndUpdateExample(Lesson lesson, int exampleId,
        CourseExampleUpsertDto upsertDto);
}
