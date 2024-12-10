using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PassedPawn.API.Filters;

// ReSharper disable once ClassNeverInstantiated.Global
public class ValidationResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasValidation =
            context.ApiDescription.ParameterDescriptions.Any(param => param.ModelMetadata?.ValidatorMetadata?.Any() == true);

        if (hasValidation)
        {
            operation.Responses.Add("422", new OpenApiResponse
            {
                Description = "Unprocessable entity - Validation error", 
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new()
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(Dictionary<string, string[]>), context.SchemaRepository)
                    }
                }
            });
        }
    }
}