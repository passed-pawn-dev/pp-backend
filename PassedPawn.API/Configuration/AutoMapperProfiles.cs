using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Entities.Courses;
using PassedPawn.Models.DTOs.Course;
using PassedPawn.Models.DTOs.Course.Example;
using PassedPawn.Models.DTOs.Course.Exercise;
using PassedPawn.Models.DTOs.Course.Lesson;
using PassedPawn.Models.DTOs.Course.Review;
using PassedPawn.Models.DTOs.Course.Video;
using PassedPawn.Models.DTOs.Nationality;
using PassedPawn.Models.DTOs.Photo;

namespace PassedPawn.API.Configuration;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<PhotoUpsertDto, Photo>();
        CreateMap<Photo, PhotoDto>();

        CreateMap<NationalityUpsertDto, Nationality>();
        CreateMap<Nationality, NationalityDto>();

        CreateMap<CourseUpsertDto, Course>();
        CreateMap<Course, CourseDto>();

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
    }
}
