using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using PassedPawn.DataAccess;
using Microsoft.EntityFrameworkCore;
namespace PassedPawn.API.Controllers;
public class HealthController : ApiControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public HealthController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [SwaggerOperation(
        Summary = "Health check.",
        Description = "Verify api connectivity."
    )]
    public async Task<IActionResult> CheckHealth() {
        try
        {
            var result = await _dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
            
            return Ok(new 
            {
                Status = "Healthy",
                Details = "Api and database operational"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new 
            {
                Status = "Unhealthy",
                Details = "Database query failed"
            });
        }
    }
}
