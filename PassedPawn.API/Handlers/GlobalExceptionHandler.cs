using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.BusinessLogic.Exceptions;


namespace PassedPawn.API.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            KeycloakNullResponseException ex => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Keycloak response is empty"
            },
            InvalidNationalityIdException ex => new ProblemDetails
            {
              Status  = StatusCodes.Status400BadRequest,
              Title = "Invalid nationality Id"
            },
            // Wildcard. Catch specific exceptions above.
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            }
        };

        // Ensure that status was provided above. Exception is thrown otherwise
        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}