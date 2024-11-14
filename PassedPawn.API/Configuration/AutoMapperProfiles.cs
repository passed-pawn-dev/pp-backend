using AutoMapper;
using PassedPawn.DataAccess.Entities;
using PassedPawn.Models.DTOs.Keycloak;
using PassedPawn.Models.DTOs.Nationality;
using PassedPawn.Models.DTOs.Photo;
using PassedPawn.Models.DTOs.User.Student;

namespace PassedPawn.API.Configuration;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<StudentUpsertDto, Student>();
        CreateMap<Student, StudentUpsertDto>();
        
        CreateMap<StudentUpsertDto, UserRegistrationDto>()
            .ForMember(dest => dest.Credentials,
                opt => opt.MapFrom(src => new List<CredentialDto>() {new CredentialDto() { Value = src.Password }} ));
            
        CreateMap<PhotoUpsertDto, Photo>();
        CreateMap<Photo, PhotoDto>();

        CreateMap<NationalityUpsertDto, Nationality>();
        CreateMap<Nationality, NationalityDto>();
    }
}
