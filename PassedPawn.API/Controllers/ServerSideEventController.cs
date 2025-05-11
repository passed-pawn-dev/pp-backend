using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.BusinessLogic.Services.Contracts;

namespace PassedPawn.API.Controllers;

[ApiController]
[Authorize]
[Route("api/sse")]
public class ServerSideEventController(ISseUserConnectionManager sseUserConnectionManager,
    IClaimsPrincipalService claimsPrincipalService) : ControllerBase
{
    [HttpGet]
    public async Task Get(CancellationToken cancellationToken, [FromQuery] int interval = 20)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");
        
        var userId = await claimsPrincipalService.GetStudentId(User);
        sseUserConnectionManager.AddConnection(userId, Response);

        while (cancellationToken.IsCancellationRequested is false)
            await Task.Delay(interval * 1000, cancellationToken);
        
        sseUserConnectionManager.RemoveConnection(userId);
    }
}
