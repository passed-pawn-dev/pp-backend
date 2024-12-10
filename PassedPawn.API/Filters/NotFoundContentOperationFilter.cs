using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PassedPawn.API.Filters;

public class NotFoundContentOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses?.ContainsKey("404") == true)
        {
            operation.Responses["404"].Content.Clear();
        }
    }
}