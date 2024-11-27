using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Review;
using Swashbuckle.AspNetCore.Annotations;

namespace PassedPawn.API.Controllers;

public class CourseReviewController(IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseReviewDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Returns single course review by id"
    )]
    public async Task<IActionResult> GetReview(int id)
    {
        var review = await unitOfWork.CourseReviews.GetByIdAsync<CourseReviewDto>(id);

        if (review is null)
            return NotFound();

        return Ok(review);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseReviewDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Updates a course review"
    )]
    public async Task<IActionResult> UpdateReview(int id, CourseReviewUpsertDto reviewUpsertDto, IMapper mapper)
    {
        var review = await unitOfWork.CourseReviews.GetByIdAsync(id);

        if (review is null)
            return NotFound();

        mapper.Map(reviewUpsertDto, review);
        unitOfWork.CourseReviews.Update(review);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        var reviewDto = mapper.Map<CourseReviewDto>(review);
        return Ok(reviewDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Deletes a course review"
    )]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await unitOfWork.CourseReviews.GetByIdAsync(id);

        if (review is null)
            return NotFound();

        unitOfWork.CourseReviews.Delete(review);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }
}