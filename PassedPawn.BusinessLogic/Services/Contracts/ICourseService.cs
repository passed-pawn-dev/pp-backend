using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICourseService
{
    public Task<ServiceResult<CourseDto>> ValidateAndAddCourse(CourseUpsertDto courseUpsertDto);
    public Task<ServiceResult<CourseDto>> ValidateAndUpdateCourse(Course course, CourseUpsertDto courseUpsertDto);
    public Task<ServiceResult<LessonDto>> ValidateAndAddLesson(Course course, LessonUpsertDto lessonUpsertDto);
    public Task<ServiceResult<LessonDto>> ValidateAndUpdateLesson(Course course, int lessonId, LessonUpsertDto lessonUpsertDto);
    public Task<CourseReviewDto> AddReview(Course course, CourseReviewUpsertDto reviewUpsertDto);
}
