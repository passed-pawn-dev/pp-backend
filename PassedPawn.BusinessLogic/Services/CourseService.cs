using AutoMapper;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;

namespace PassedPawn.BusinessLogic.Services;

public class CourseService(IUnitOfWork unitOfWork, IMapper mapper) : ICourseService
{
    public async Task<ServiceResult<CourseDto>> ValidateAndAddCourse(CourseUpsertDto courseUpsertDto)
    {
        var errors = ValidateLessonNumbers(courseUpsertDto);

        if (errors.Count != 0)
            return ServiceResult<CourseDto>.Failure(errors);
        
        var course = mapper.Map<Course>(courseUpsertDto);
        unitOfWork.Courses.Add(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseDto>.Success(mapper.Map<CourseDto>(course));
    }

    public async Task<ServiceResult<CourseDto>> ValidateAndUpdateCourse(Course course, CourseUpsertDto courseUpsertDto)
    {
        var errors = ValidateLessonNumbers(courseUpsertDto);

        if (errors.Count != 0)
            return ServiceResult<CourseDto>.Failure(errors);

        
        mapper.Map(courseUpsertDto, course);
        unitOfWork.Courses.Update(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return ServiceResult<CourseDto>.Success(mapper.Map<CourseDto>(course));
    }

    public async Task<ServiceResult<LessonDto>> ValidateAndAddLesson(Course course, LessonUpsertDto lessonUpsertDto)
    {
        var lesson = mapper.Map<Lesson>(lessonUpsertDto);
        course.Lessons.Add(lesson);

        var errors = ValidateLessonNumbers(course);

        if (errors.Count != 0)
            return ServiceResult<LessonDto>.Failure(errors);

        unitOfWork.Courses.Update(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");
        
        return ServiceResult<LessonDto>.Success(mapper.Map<LessonDto>(lesson));
    }

    public async Task<ServiceResult<LessonDto>> ValidateAndUpdateLesson(Course course, int lessonId,
        LessonUpsertDto lessonUpsertDto)
    {
        var lesson = course.Lessons.Single(lesson => lesson.Id == lessonId);
        mapper.Map(lessonUpsertDto, lesson);

        var errors = ValidateLessonNumbers(course);

        if (errors.Count != 0)
            return ServiceResult<LessonDto>.Failure(errors);

        unitOfWork.Courses.Update(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");
        
        return ServiceResult<LessonDto>.Success(mapper.Map<LessonDto>(lesson));
    }

    private static List<string> ValidateLessonNumbers(CourseUpsertDto courseUpsertDto)
    {
        return courseUpsertDto.Lessons
            .Select(lesson => lesson.LessonNumber)
            .Order()
            .Select((value, index) => (value, index))
            .Aggregate(new List<string>(), (acc, curr) =>
                curr.value != curr.index + 1 ? [..acc, $"{curr.value} lesson number is incorrect."] : acc);
    }
    
    private static List<string> ValidateLessonNumbers(Course course)
    {
        return course.Lessons
            .Select(lesson => lesson.LessonNumber)
            .Order()
            .Select((value, index) => (value, index))
            .Aggregate(new List<string>(), (acc, curr) =>
                curr.value != curr.index + 1 ? [..acc, $"{curr.value} lesson number is incorrect."] : acc);
    }
}
