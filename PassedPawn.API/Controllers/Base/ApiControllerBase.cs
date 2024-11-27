using Microsoft.AspNetCore.Mvc;

namespace PassedPawn.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase;