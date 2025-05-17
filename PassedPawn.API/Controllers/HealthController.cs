using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using PassedPawn.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace PassedPawn.API.Controllers;

public class HealthController(ApplicationDbContext dbContext): ApiControllerBase
{
    [HttpGet("Readiness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [SwaggerOperation(
        Summary = "Readiness check.",
        Description = "Verify if api accepts traffic and can query the database"
    )]
    public async Task<IActionResult> CheckReadiness() 
    {
        try
        {
            await dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
            
            return Ok(new 
            {
                Status = "Healthy",
                Details = "Api and database operational"
            });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new 
            {
                Status = "Unhealthy",
                Details = "Database query failed"
            });
        }
    }

    [HttpGet("Liveness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Liveness check.",
        Description = "Verify if api accepts traffic"
    )]
    public IActionResult CheckLiveness() 
    {       
        return Ok(new 
        {
            Status = "Healthy",
            Details = "Api operational"
        });
    }
}
