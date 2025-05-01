using AutoMapper;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Exercise;

namespace PassedPawn.BusinessLogic.Services;

public class CourseExerciseService(IUnitOfWork unitOfWork, IMapper mapper) : CourseElementService,
    ICourseExerciseService
{
    public async Task<ServiceResult<CourseExerciseDto>> ValidateAndAddExercise(Lesson lesson,
        CourseExerciseUpsertDto upsertDto)
    {
        var exercise = mapper.Map<CourseExercise>(upsertDto);
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (exercise.Order > highestOrderNumber + 1 || exercise.Order < 1)
            return ServiceResult<CourseExerciseDto>.Failure([
                $"New exercise has wrong order. Maximum of {highestOrderNumber + 1} expected"
            ]);

        MoveOrderOnAdd(lesson, exercise.Order);
        lesson.Exercises.Add(exercise);
        unitOfWork.Lessons.Update(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseExerciseDto>.Success(mapper.Map<CourseExerciseDto>(exercise));
    }

    public async Task<ServiceResult<CourseExerciseDto>> ValidateAndUpdateExercise(Lesson lesson, int exampleId,
        CourseExerciseUpsertDto upsertDto)
    {
        var highestOrderNumber = GetHighestOrderNumber(lesson);

        if (upsertDto.Order > highestOrderNumber || upsertDto.Order < 1)
            return ServiceResult<CourseExerciseDto>.Failure([
                $"New exercise has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        var exercise = lesson.Exercises.Single(exercise => exercise.Id == exampleId);
        MoveOrderOnUpdate(lesson, exercise.Order, upsertDto.Order);
        mapper.Map(upsertDto, exercise);
        unitOfWork.Puzzles.Update(exercise);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseExerciseDto>.Success(mapper.Map<CourseExerciseDto>(exercise));
    }

    public async Task DeleteExercise(Lesson lesson, CourseExercise courseExercise)
    {
        unitOfWork.Puzzles.Delete(courseExercise);
        MoveOrderOnDelete(lesson, courseExercise.Order);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");
    }
}
