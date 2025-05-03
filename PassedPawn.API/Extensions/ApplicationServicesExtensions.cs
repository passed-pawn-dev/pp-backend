using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PassedPawn.API.Configuration;
using PassedPawn.BusinessLogic.Services;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess;
using PassedPawn.DataAccess.Repositories;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.Keyclock;

namespace PassedPawn.API.Extensions;

public static class ApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(AutoMapperProfiles));

        services.AddSingleton<ICloudinaryService, CloudinaryService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ICourseExampleService, CourseExampleService>();
        services.AddScoped<ICoursePuzzleService, CoursePuzzleService>();
        services.AddScoped<ICourseVideoService, CourseVideoService>();
        services.AddScoped<IKeycloakService, KeycloakService>();
        services.AddScoped<IClaimsPrincipalService, ClaimsPrincipalService>();
        services.AddScoped<ICourseQuizService, CourseQuizService>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                x =>
                {
                    x.MigrationsAssembly("PassedPawn.DataAccess");
                    x.EnableRetryOnFailure();
                }
            );
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakConfiguration = configuration.GetSection("Keycloak");
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = $"{keycloakConfiguration["Authority"]}";
                
                options.Audience = "account";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuers =
                    [
                        $"{keycloakConfiguration["Issuer"]}"
                    ]
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.Principal?.Identity is not ClaimsIdentity claimsIdentity)
                            return Task.CompletedTask;
                        
                        var resourceRoles = context.Principal.FindFirst("resource_access")?.Value;
                        
                        if (resourceRoles is null)
                            return Task.CompletedTask;
                        
                        var deserializedResourceRoles = JsonSerializer.Deserialize<ResourceAccess>(resourceRoles);
                        var roles = deserializedResourceRoles?.ApiClient?.Roles;

                        foreach (var role in roles ?? [])
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

                        return Task.CompletedTask;
                    }
                };
            });
        
        services
            .AddAuthorization()
            .AddAuthorizationBuilder()
            .AddPolicy("require student role", policy => policy.RequireRole("student"))
            .AddPolicy("require coach role", policy => policy.RequireRole("coach"));
    }
}