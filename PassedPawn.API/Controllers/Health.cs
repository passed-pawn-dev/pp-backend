using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
namespace PassedPawn.API.Controllers;

public class HealthController() : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Health check.",
        Description = "Verify api connectivity."
    )]
    public async Task<IActionResult> CheckHealth() {
      return Ok();
    }
}
