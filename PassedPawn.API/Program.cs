using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PassedPawn.API.Extensions;
using PassedPawn.API.Handlers;
using PassedPawn.DataAccess;
using PassedPawn.Models.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
});

builder.Services.AddOptions<KeycloakConfig>()
    .BindConfiguration("Keycloak")
    .ValidateDataAnnotations().ValidateOnStart();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();



app.Run();
app.Run();