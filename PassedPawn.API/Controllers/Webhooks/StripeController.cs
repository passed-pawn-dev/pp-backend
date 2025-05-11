using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using Stripe;

namespace PassedPawn.API.Controllers.Webhooks;

public class StripeController(IConfiguration configuration) : ApiControllerBase
{
    private readonly string _endpointSecret = configuration["Stripe:PaymentSuccessfulEndpointSecret"]!;

    [HttpPost("course-bought")]
    public async Task<IActionResult> CourseBought()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _endpointSecret);

            Console.WriteLine(stripeEvent.Type);
            Console.WriteLine(stripeEvent.Data);
            
            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
}