using Microsoft.EntityFrameworkCore;
using PassedPawn.API.Configuration;
using PassedPawn.DataAccess;
using PassedPawn.DataAccess.Repositories;
using PassedPawn.DataAccess.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("PassedPawn.DataAccess")
        );
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("test", () => "hello world");

app.Run();