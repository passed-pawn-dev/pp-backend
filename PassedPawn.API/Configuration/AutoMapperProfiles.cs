using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.DataAccess.Entities.Courses.Elements;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Example.Move;
using PassedPawn.Models.DTOs.Course.Example.Move.Arrow;
using PassedPawn.Models.DTOs.Course.Example.Move.Highlight;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Puzzle;
using PassedPawn.Models.DTOs.Course.Quiz;
using PassedPawn.Models.DTOs.Course.Review;
using PassedPawn.Models.DTOs.Course.Video;
using PassedPawn.Models.DTOs.Keycloak;
using PassedPawn.Models.DTOs.Nationality;
using PassedPawn.Models.DTOs.Photo;
using PassedPawn.Models.DTOs.User.Coach;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.API.Configuration;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<StudentUpsertDto, Student>()
            .ForMember(dest => dest.ChessTitle, opt => opt.MapFrom(src => src.ChessTitle == null ? null : src.ChessTitle.ToString()));
        
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.ChessTitle, opt => opt.MapFrom(src => src.ChessTitle == null ? null : src.ChessTitle.ToString()));

        CreateMap<StudentUpsertDto, UserRegistrationDto>()
            .ForMember(dest => dest.Credentials,
                opt => opt.MapFrom(src => new List<CredentialDto> { new() { Value = src.Password } }));
        
        CreateMap<CoachUpsertDto, Coach>();
        CreateMap<Coach, CoachDto>();
        CreateMap<Coach, CoachProfileDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => FullName(src)))
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality == null ? null : src.Nationality.FullName))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photo == null ? null : src.Photo.Url));

        CreateMap<CoachUpsertDto, UserRegistrationDto>()
            .ForMember(dest => dest.Credentials,
                opt => opt.MapFrom(src => new List<CredentialDto> { new() { Value = src.Password } }));

        CreateMap<PhotoUpsertDto, Photo>();
        CreateMap<Photo, PhotoDto>();

        CreateMap<NationalityUpsertDto, Nationality>();
        CreateMap<Nationality, NationalityDto>();

        CreateMap<CourseUpsertDto, Course>();
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.CoachName, opt => opt.MapFrom(src => FullName(src.Coach!)))
            .ForMember(dest => dest.AverageScore, opt => opt.MapFrom(src => AverageScore(src.Reviews)))
            .ForMember(dest => dest.EnrolledStudentsCount, opt => opt.MapFrom(src => src.Students.Count));

        CreateMap<Course, BoughtCourseDto>()
            .ForMember(dest => dest.CoachName, opt => opt.MapFrom(src => FullName(src.Coach!)));
        
        CreateMap<Course, NonBoughtCourseDetailsDto>()
            .ForMember(dest => dest.PuzzleCount,
                opt => opt.MapFrom(src => src.Lessons.Sum(lesson => lesson.Puzzles.Count)))
            .ForMember(dest => dest.ReviewCount,
                opt => opt.MapFrom(src => src.Lessons.Sum(lesson => lesson.Videos.Count)))
            .ForMember(dest => dest.QuizCount,
                opt => opt.MapFrom(src => src.Lessons.Sum(lesson => lesson.Quizzes.Count)))
            .ForMember(dest => dest.ExampleCount,
                opt => opt.MapFrom(src => src.Lessons.Sum(lesson => lesson.Examples.Count)))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
            .ForMember(dest => dest.AverageScore, opt => opt.MapFrom(src => AverageScore(src.Reviews)))
            .ForMember(dest => dest.EnrolledStudentsCount, opt => opt.MapFrom(src => src.Students.Count))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ReleaseDate)));

        CreateMap<Course, BoughtCourseDetailsDto>();

        CreateMap<Course, CoachCourseDto>()
            .ForMember(dest => dest.AverageScore, opt => opt.MapFrom(src => AverageScore(src.Reviews)))
            .ForMember(dest => dest.LessonCount, opt => opt.MapFrom(src => src.Lessons.Count))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Thumbnail == null ? null : src.Thumbnail.Url))
            .ForMember(dest => dest.ElementCount, opt => opt.MapFrom(src =>
                src.Lessons.Sum(lesson => lesson.Examples.Count + lesson.Puzzles.Count + lesson.Videos.Count + lesson.Quizzes.Count)));

        CreateMap<Course, CourseEditViewDto>()
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Thumbnail == null ? null : src.Thumbnail.Url));

        CreateMap<Course, CoachCourseDetailsDto>()
            .ForMember(dest => dest.AverageScore, opt => opt.MapFrom(src => AverageScore(src.Reviews)))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Thumbnail == null ? null : src.Thumbnail.Url))
            .ForMember(dest => dest.EnrolledStudentsCount, opt => opt.MapFrom(src => src.Students.Count));

        CreateMap<Lesson, CoachCourseDetailsLessonDto>();

        CreateMap<CourseQuiz, CoachCourseDetailsLessonElementDto>();
        CreateMap<CourseExample, CoachCourseDetailsLessonElementDto>();
        CreateMap<CoursePuzzle, CoachCourseDetailsLessonElementDto>();
        CreateMap<CourseVideo, CoachCourseDetailsLessonElementDto>();
        
        CreateMap<Lesson, BoughtCourseDetailsLessonDto>();

        CreateMap<CourseQuiz, BoughtCourseDetailsLessonElementSlimDto>();
        CreateMap<CourseExample, BoughtCourseDetailsLessonElementSlimDto>();
        CreateMap<CourseVideo, BoughtCourseDetailsLessonElementSlimDto>();
        CreateMap<CoursePuzzle, BoughtCourseDetailsLessonElementSlimDto>();

        CreateMap<Coach, NonBoughtCourseDetailsCoachDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => FullName(src)))
            .ForMember(dest => dest.CreatedCoursesCount, opt => opt.MapFrom(src => src.Courses.Count))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Photo == null ? null : src.Photo.Url));
        
        CreateMap<LessonUpsertDto, Lesson>();
        CreateMap<Lesson, LessonDto>();

        CreateMap<CourseVideoAddDto, CourseVideo>();
        CreateMap<CourseVideoUpdateDto, CourseVideo>();
        CreateMap<CourseVideo, CourseVideoDto>();

        CreateMap<CoursePuzzleUpsertDto, CoursePuzzle>();
        CreateMap<CoursePuzzle, CoursePuzzlesDto>();

        CreateMap<CourseExampleUpsertDto, CourseExample>()
            .ForMember(dest => dest.Moves, opt => opt.MapFrom(src => src.Steps));
        CreateMap<CourseExample, CourseExampleDto>()
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.Moves));

        CreateMap<CourseReviewUpsertDto, CourseReview>();
        CreateMap<CourseReview, CourseReviewDto>();

        CreateMap<CoursePuzzle, CoursePuzzlesDto>();
        CreateMap<CoursePuzzleUpsertDto, CoursePuzzle>();

        CreateMap<CourseExampleMove, CourseExampleMoveDto>();
        CreateMap<CourseExampleMoveUpsertDto, CourseExampleMove>();
        
        CreateMap<CourseExampleMoveArrow, CourseExampleMoveArrowDto>();
        CreateMap<CourseExampleMoveArrowUpsertDto, CourseExampleMoveArrow>();
        
        CreateMap<CourseExampleMoveHighlight, CourseExampleMoveHighlightDto>();
        CreateMap<CourseExampleMoveHighlightUpsertDto, CourseExampleMoveHighlight>();

        CreateMap<CourseQuiz, CourseQuizDto>()
            .ForMember(dest => dest.Solution, opt => opt.MapFrom(src => src.Number));
        
        CreateMap<CourseQuizUpsertDto, CourseQuiz>();
        
        CreateMap<QuizAnswer, AnswerDto>();
        CreateMap<AnswerUpsertDto, QuizAnswer>();
    }

    private static double AverageScore(ICollection<CourseReview> reviews) =>
        reviews.Count > 0 ? reviews.Average(review => review.Value) : 0;

    private static string FullName(User coach) => $"{coach.FirstName} {coach.LastName}";
}