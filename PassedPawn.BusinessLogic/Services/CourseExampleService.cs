using AutoMapper;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Example;

namespace PassedPawn.BusinessLogic.Services;

public class CourseExampleService(IUnitOfWork unitOfWork, IMapper mapper) : CourseElementService, ICourseExampleService
{
    public async Task<ServiceResult<CourseExampleDto>> ValidateAndAddExample(Lesson lesson,
        CourseExampleUpsertDto upsertDto)
    {
        var example = mapper.Map<CourseExample>(upsertDto);
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (example.Order > highestOrderNumber + 1 || example.Order < 1)
            return ServiceResult<CourseExampleDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber + 1} expected"
            ]);

        MoveOrderOnAdd(lesson, example.Order);
        lesson.Examples.Add(example);
        unitOfWork.Lessons.Update(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseExampleDto>.Success(mapper.Map<CourseExampleDto>(example));
    }

    public async Task<ServiceResult<CourseExampleDto>> ValidateAndUpdateExample(Lesson lesson, int exampleId,
        CourseExampleUpsertDto upsertDto)
    {
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (upsertDto.Order > highestOrderNumber || upsertDto.Order < 1)
            return ServiceResult<CourseExampleDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        var example = lesson.Examples.Single(example => example.Id == exampleId);
        MoveOrderOnUpdate(lesson, example.Order, upsertDto.Order);
        mapper.Map(upsertDto, example);
        unitOfWork.Examples.Update(example);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseExampleDto>.Success(mapper.Map<CourseExampleDto>(example));
    }

    public async Task DeleteExample(Lesson lesson, CourseExample courseExample)
    {
        unitOfWork.Examples.Delete(courseExample);
        MoveOrderOnDelete(lesson, courseExample.Order);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");
    }
}
