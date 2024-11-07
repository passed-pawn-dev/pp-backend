using AutoMapper;
using PassedPawn.DataAccess.Entities;
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
    }
}
