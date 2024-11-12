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
        var highestLessonNumber = GetHighestLessonNumber(course);

        if (lesson.LessonNumber > highestLessonNumber + 1)
            return ServiceResult<LessonDto>.Failure([
                $"New lesson has wrong order. Maximum of {highestLessonNumber + 1} expected"
            ]);
        
        MoveLessonNumbersOnAdd(course, lesson.LessonNumber);
        course.Lessons.Add(lesson);
        unitOfWork.Courses.Update(course);
        
        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");
        
        return ServiceResult<LessonDto>.Success(mapper.Map<LessonDto>(lesson));
    }

    public async Task<ServiceResult<LessonDto>> ValidateAndUpdateLesson(Course course, int lessonId,
        LessonUpsertDto lessonUpsertDto)
    {
        var highestLessonNumber = GetHighestLessonNumber(course);

        if (lessonUpsertDto.LessonNumber > highestLessonNumber)
            return ServiceResult<LessonDto>.Failure([
                $"New lesson has wrong order. Maximum of {highestLessonNumber} expected"
            ]);

        var lesson = course.Lessons.Single(lesson => lesson.Id == lessonId);
        MoveLessonNumbersOnUpdate(course, lesson.LessonNumber, lessonUpsertDto.LessonNumber);
        mapper.Map(lessonUpsertDto, lesson);
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

    private static void MoveLessonNumbersOnAdd(Course course, int newLessonNumber)
    {
        foreach (var lesson in course.Lessons)
        {
            if (lesson.LessonNumber < newLessonNumber)
                continue;

            lesson.LessonNumber++;
        }
    }

    private static void MoveLessonNumbersOnUpdate(Course course, int oldLessonNumber, int newLessonNumber)
    {
        if (oldLessonNumber == newLessonNumber)
            return;

        if (newLessonNumber > oldLessonNumber)
        {
            foreach (var lesson in course.Lessons)
            {
                if (lesson.LessonNumber <= oldLessonNumber || lesson.LessonNumber > newLessonNumber)
                    continue;

                lesson.LessonNumber--;
            }
            
            return;
        }
        
        
        foreach (var lesson in course.Lessons)
        {
            if (lesson.LessonNumber < newLessonNumber || lesson.LessonNumber >= oldLessonNumber)
                continue;

            lesson.LessonNumber++;
        }
    }

    private static int GetHighestLessonNumber(Course course)
    {
        return course.Lessons
            .Select(lesson => lesson.LessonNumber)
            .Max();
    }
}
