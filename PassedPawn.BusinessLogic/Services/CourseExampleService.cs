using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        var highestOrderNumber = GetHighestOrderNumber(lesson) + 1;
        upsertDto.Order ??= highestOrderNumber;
        var example = mapper.Map<CourseExample>(upsertDto);

        if (example.Order > highestOrderNumber || example.Order < 1)
            return ServiceResult<CourseExampleDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        if (!CorrectMoveOrders(example))
            return ServiceResult<CourseExampleDto>.Failure([
                "Invalid order of moves. They need to be 1 to n"
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
        upsertDto.Order ??= highestOrderNumber;

        if (upsertDto.Order > highestOrderNumber || upsertDto.Order < 1)
            return ServiceResult<CourseExampleDto>.Failure([
                $"New example has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        var example = lesson.Examples.Single(example => example.Id == exampleId);
        
        if (!CorrectMoveOrders(example))
            return ServiceResult<CourseExampleDto>.Failure([
                "Invalid order of moves. They need to be 1 to n"
            ]);
        
        MoveOrderOnUpdate(lesson, example.Order, upsertDto.Order.Value);

        mapper.Map(upsertDto, example);

        await unitOfWork.CourseExampleMoves.DeleteAllForExampleAsync(exampleId);
        
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
    
    private static bool CorrectMoveOrders(CourseExample courseExample)
    {
        var sortedNumbers = courseExample.Moves
            .Select(lesson => lesson.Order)
            .Order()
            .ToArray();

        return !sortedNumbers.Where((t, i) => i + 1 != t).Any();
    }
}
