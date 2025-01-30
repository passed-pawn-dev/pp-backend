using PassedPawn.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Extensions;
using PassedPawn.API.Filters;
using PassedPawn.API.Handlers;
using PassedPawn.Models.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationServices(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
    options.OperationFilter<ValidationResponseOperationFilter>();
    options.OperationFilter<NotFoundContentOperationFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        // Extract the validation errors
        var errors = context.ModelState
            .Where(m => m.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        // Create a custom response
        var customResponse = new
        {
            Errors = errors
        };

        // Return a 422 status code if applicable
        return new ObjectResult(customResponse)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity // 422 status code
        };
    };
});

builder.Services.AddOptions<KeycloakConfig>()
    .BindConfiguration("Keycloak")
    .ValidateDataAnnotations().ValidateOnStart();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();