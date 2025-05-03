using System.Security.Claims;
using AutoMapper;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course.Puzzle;

namespace PassedPawn.BusinessLogic.Services;

public class CoursePuzzleService(IUnitOfWork unitOfWork, IMapper mapper,
    IClaimsPrincipalService claimsPrincipalService) : CourseElementService, ICoursePuzzleService
{
    public async Task<ServiceResult<CoursePuzzlesDto>> ValidateAndAddPuzzle(Lesson lesson,
        CoursePuzzleUpsertDto upsertDto)
    {
        var highestOrderNumber = GetHighestOrderNumber(lesson) + 1;
        upsertDto.Order ??= highestOrderNumber;
        var puzzle = mapper.Map<CoursePuzzle>(upsertDto);

        if (puzzle.Order > highestOrderNumber || puzzle.Order < 1)
            return ServiceResult<CoursePuzzlesDto>.Failure([
                $"New puzzle has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        MoveOrderOnAdd(lesson, puzzle.Order);
        lesson.Puzzles.Add(puzzle);
        unitOfWork.Lessons.Update(lesson);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CoursePuzzlesDto>.Success(mapper.Map<CoursePuzzlesDto>(puzzle));
    }

    public async Task<ServiceResult<CoursePuzzlesDto>> ValidateAndUpdatePuzzle(Lesson lesson, int exampleId,
        CoursePuzzleUpsertDto upsertDto)
    {
        var highestOrderNumber = GetHighestOrderNumber(lesson);
        upsertDto.Order ??= highestOrderNumber;

        if (upsertDto.Order > highestOrderNumber || upsertDto.Order < 1)
            return ServiceResult<CoursePuzzlesDto>.Failure([
                $"New puzzle has wrong order. Maximum of {highestOrderNumber} expected"
            ]);

        var puzzle = lesson.Puzzles.Single(puzzle => puzzle.Id == exampleId);
        MoveOrderOnUpdate(lesson, puzzle.Order, upsertDto.Order.Value);
        mapper.Map(upsertDto, puzzle);
        unitOfWork.Puzzles.Update(puzzle);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CoursePuzzlesDto>.Success(mapper.Map<CoursePuzzlesDto>(puzzle));
    }

    public async Task DeletePuzzle(Lesson lesson, CoursePuzzle coursePuzzle)
    {
        unitOfWork.Puzzles.Delete(coursePuzzle);
        MoveOrderOnDelete(lesson, coursePuzzle.Order);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");
    }
    
    public async Task<ServiceResult<string>> CheckPuzzleSolution(ClaimsPrincipal user,
        int puzzleId, string puzzleSolution)
    {
        var puzzle = await unitOfWork.Puzzles.GetPuzzleById(puzzleId);

        if (puzzle is null)
            return ServiceResult<string>.Failure(["Puzzle not found"]);

        var student = await claimsPrincipalService.GetStudent(user);

        if (puzzle.Students.Contains(student))
            return ServiceResult<string>.Failure(["Already solved this puzzle"]);

        if (puzzleSolution.Split(',').Length % 2 == 0)
            return ServiceResult<string>.Failure(["Invalid number of moves"]);
        
        if (puzzleSolution == puzzle.Solution)
        {
            student.Puzzles.Add(puzzle);
            unitOfWork.Students.Update(student);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<string>.Success("Puzzle solved successfully");
        }
        
        // e4,Nxb2,e5
        if (puzzle.Solution.StartsWith(puzzleSolution))
        {
            var nextMove = puzzle.Solution
                .Skip(puzzleSolution.Length + 1)
                .TakeWhile(letter => letter != ',')
                .Aggregate(string.Empty, (current, letter) => current + letter);

            return ServiceResult<string>.Success(nextMove);
        }

        return ServiceResult<string>.Failure(["Invalid solution"]);
    }
}
