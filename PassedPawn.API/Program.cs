using Microsoft.EntityFrameworkCore;
using PassedPawn.API.Configuration;
using PassedPawn.API.Handlers;
using PassedPawn.DataAccess;
using PassedPawn.DataAccess.Repositories;
using PassedPawn.DataAccess.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapGet("test", () => "hello world");
app.MapControllers();

app.Run();