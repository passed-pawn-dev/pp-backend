using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Exercise;
using PassedPawn.Models.DTOs.Course.Lesson;
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

        CreateMap<CoachUpsertDto, UserRegistrationDto>()
            .ForMember(dest => dest.Credentials,
                opt => opt.MapFrom(src => new List<CredentialDto> { new() { Value = src.Password } }));

        CreateMap<PhotoUpsertDto, Photo>();
        CreateMap<Photo, PhotoDto>();

        CreateMap<NationalityUpsertDto, Nationality>();
        CreateMap<Nationality, NationalityDto>();

        CreateMap<CourseUpsertDto, Course>();
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.LessonNumber, opt => opt.MapFrom(src => src.Lessons.Count))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Reviews.Count > 0 ? src.Reviews.Average(review => review.Value) : 0));

        CreateMap<Course, NonUserCourse>()
            .ForMember(dest => dest.LessonNumber, opt => opt.MapFrom(src => src.Lessons.Count))
            .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.Students.Count));

        CreateMap<Course, CourseDetails>();
            
        CreateMap<LessonUpsertDto, Lesson>();
        CreateMap<Lesson, LessonDto>();

        CreateMap<VideoUpsertDto, CourseVideo>();
        CreateMap<CourseVideo, VideoDto>();

        CreateMap<CourseExerciseUpsertDto, CourseExercise>();
        CreateMap<CourseExercise, CourseExerciseDto>();

        CreateMap<CourseExampleUpsertDto, CourseExample>();
        CreateMap<CourseExample, CourseExampleDto>();

        CreateMap<CourseReviewUpsertDto, CourseReview>();
        CreateMap<CourseReview, CourseReviewDto>();

        CreateMap<CourseExercise, CourseExerciseDto>();
        CreateMap<CourseExerciseUpsertDto, CourseExercise>();

        CreateMap<Course, UserCourseDto>()
            .ForMember(dest => dest.LessonNumber, opt => opt.MapFrom(src => src.Lessons.Count));
    }
}