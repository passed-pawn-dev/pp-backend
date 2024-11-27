using Microsoft.EntityFrameworkCore;
using PassedPawn.API.Configuration;
using PassedPawn.BusinessLogic.Services;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess;
using PassedPawn.DataAccess.Repositories;
using PassedPawn.DataAccess.Repositories.Contracts;

namespace PassedPawn.API.Extensions;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(AutoMapperProfiles));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IKeycloakService, KeycloakService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("PassedPawn.DataAccess")
            );
        });
    }
}