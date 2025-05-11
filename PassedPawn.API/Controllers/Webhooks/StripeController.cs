using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.DataAccess.Repositories.Contracts;
using Stripe;

namespace PassedPawn.API.Controllers.Webhooks;

public class StripeController(IConfiguration configuration, IUnitOfWork unitOfWork,
    ISseUserConnectionManager sseUserConnectionManager) : ApiControllerBase
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

            if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
                return BadRequest();
            
            var metadata = paymentIntent.Metadata;
            var userId = metadata["userId"];
            var courseId = metadata["courseId"];

            if (userId is null || courseId is null)
                return BadRequest();

            var student = await unitOfWork.Students.GetByIdAsync(int.Parse(userId));
            var course = await unitOfWork.Courses.GetWithStudentsById(int.Parse(courseId));

            if (student is null || course is null)
            {
                await sseUserConnectionManager.SendEventAsync(int.Parse(userId), $"Failed to buy ${courseId}");
                throw new Exception(); // TODO: Refund user
            }

            if (course.Students.Contains(student))
            {
                await sseUserConnectionManager.SendEventAsync(int.Parse(userId), $"Failed to buy ${courseId}");
                return Conflict("Course already on the list"); // TODO: Refund user
            }
        
            course.Students.Add(student);
            await unitOfWork.SaveChangesAsync();
            await sseUserConnectionManager.SendEventAsync(int.Parse(userId), $"Bought ${courseId}");
            return NoContent();
        }
        catch (StripeException e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
}