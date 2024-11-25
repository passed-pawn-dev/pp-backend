using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PassedPawn.API.Controllers.Base;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Review;

namespace PassedPawn.API.Controllers;

public class CourseReviewController(IUnitOfWork unitOfWork) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetReview(int id)
    {
        var review = await unitOfWork.CourseReviews.GetByIdAsync<CourseReviewDto>(id);

        if (review is null)
            return NotFound();

        return Ok(review);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReview(int id, CourseReviewUpsertDto reviewUpsertDto, IMapper mapper)
    {
        CourseReview? review = await unitOfWork.CourseReviews.GetByIdAsync(id);

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
    public async Task<IActionResult> DeleteReview(int id)
    {
        CourseReview? review = await unitOfWork.CourseReviews.GetByIdAsync(id);

        if (review is null)
            return NotFound();

        unitOfWork.CourseReviews.Delete(review);

        if (!await unitOfWork.SaveChangesAsync())
            throw new Exception("Failed to save database");

        return NoContent();
    }
}