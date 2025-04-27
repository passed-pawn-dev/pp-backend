using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Review;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseController(IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpGet("{courseId:int}/review")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseReviewDto>))]
    [SwaggerOperation(
        Summary = "Returns all reviews that belong to a course"
    )]
    public async Task<IActionResult> GetReviews(int courseId)
    {
        var reviews = await unitOfWork.CourseReviews
            .GetAllWhereAsync<CourseReviewDto>(review => review.CourseId == courseId);

        return Ok(reviews);
    }
}
