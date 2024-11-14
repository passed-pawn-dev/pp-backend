using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PassedPawn.API.Configuration;
using PassedPawn.API.Controllers;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Course.Review;

namespace PassedPawn.Tests.Controllers;

public class CourseReviewControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CourseReviewController _courseReviewController;

    public CourseReviewControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseReviewController = new CourseReviewController(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetReview_ShouldReturnOk_WhenReviewExists()
    {
        // Arrange
        const int id = 1;
        var courseReviewDto = new CourseReviewDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.GetByIdAsync<CourseReviewDto>(id))
            .ReturnsAsync(courseReviewDto);
        
        // Act
        var result = await _courseReviewController.GetReview(id);
        
        // Assert
        var okObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(courseReviewDto, okObject.Value);
    }

    [Fact]
    public async Task GetReview_ShouldReturnNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        const int id = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.GetByIdAsync<CourseReviewDto>(id))
            .ReturnsAsync((CourseReviewDto?)null);
        
        // Act
        var result = await _courseReviewController.GetReview(id);
        
        // Assert
       Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnOk_WhenCourseExists()
    {
        // Arrange
        const int id = 1;
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
        var mapper = new Mapper(config);
        var review = new CourseReview();
        var reviewUpsertDto = new CourseReviewUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.GetByIdAsync(id))
            .ReturnsAsync(review);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.Update(review));
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(true);
        
        // Act
        var result = await _courseReviewController.UpdateReview(id, reviewUpsertDto, mapper);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<CourseReviewDto>(okResult.Value);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        const int id = 1;
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfiles()));
        var mapper = new Mapper(config);
        var reviewUpsertDto = new CourseReviewUpsertDto();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.GetByIdAsync(id))
            .ReturnsAsync((CourseReview?)null);
        
        // Act
        var result = await _courseReviewController.UpdateReview(id, reviewUpsertDto, mapper);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturnNoContent_WhenCourseExists()
    {
        // Arrange
        const int id = 1;
        var review = new CourseReview();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.GetByIdAsync(id))
            .ReturnsAsync(review);
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.Delete(review));
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _courseReviewController.DeleteReview(id);
        
        // Assert
        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task DeleteReview_ShouldReturnNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        const int id = 1;
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CourseReviews.GetByIdAsync(id))
            .ReturnsAsync((CourseReview?)null);

        // Act
        var result = await _courseReviewController.DeleteReview(id);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
