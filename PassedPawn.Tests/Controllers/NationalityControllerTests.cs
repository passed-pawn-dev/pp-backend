using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PassedPawn.API.Controllers;
using PassedPawn.DataAccess.Entities;
using PassedPawn.DataAccess.Repositories.Contracts;
using PassedPawn.Models.DTOs.Nationality;
using PassedPawn.Models.DTOs.Photo;

namespace PassedPawn.Tests.Controllers;

public class NationalityControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly NationalityController _controller;

    private static NationalityDto SampleDto => new()
        { Id = 1, FullName = "TestNationality", ShortName = "TN", Flag = new PhotoDto { Id = 1 } };

    private static NationalityUpsertDto SampleUpsertDto => new()
        { FullName = "NewNationality", ShortName = "TN", Flag = new PhotoUpsertDto() };

    private static Nationality SampleNationality => new()
        { Id = 1, FullName = "TestNationality", ShortName = "TN", Flag = new Photo { Id = 1 } };

    public NationalityControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        // Configure AutoMapper with the necessary profiles
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Nationality, NationalityDto>();
            cfg.CreateMap<NationalityUpsertDto, Nationality>();
            cfg.CreateMap<Photo, PhotoDto>();
            cfg.CreateMap<PhotoUpsertDto, Photo>();
        });
        var mapper = config.CreateMapper();

        _controller = new NationalityController(_mockUnitOfWork.Object, mapper);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfNationalities()
    {
        // Arrange
        var nationalityDtoList = new List<NationalityDto> { SampleDto };
        _mockUnitOfWork.Setup(u => u.Nationalities.GetAllAsync<NationalityDto>())
            .ReturnsAsync(nationalityDtoList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<NationalityDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WhenNationalityExists()
    {
        // Arrange
        var nationalityDto = SampleDto;
        _mockUnitOfWork.Setup(u => u.Nationalities.GetByIdAsync<NationalityDto>(1))
            .ReturnsAsync(nationalityDto);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<NationalityDto>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNationalityDoesNotExist()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.Nationalities.GetByIdAsync<NationalityDto>(1))
            .ReturnsAsync((NationalityDto?)null);

        // Act
        var result = await _controller.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Create_ReturnsCreatedAtActionResult_WhenSuccessful()
    {
        // Arrange
        var nationalityUpsertDto = SampleUpsertDto;
        var nationality = SampleNationality;

        _mockUnitOfWork.Setup(u => u.Nationalities.Add(nationality));
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Create(nationalityUpsertDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<NationalityDto>(createdResult.Value);
        Assert.Equal(nationalityUpsertDto.FullName, returnValue.FullName);
    }

    [Fact]
    public async Task Update_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        var nationalityUpsertDto = SampleUpsertDto;
        var nationality = SampleNationality;

        _mockUnitOfWork.Setup(u => u.Nationalities.GetByIdAsync(1)).ReturnsAsync(nationality);
        _mockUnitOfWork.Setup(u => u.Nationalities.Update(nationality));
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(1, nationalityUpsertDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<NationalityDto>(okResult.Value);
        Assert.Equal("NewNationality", returnValue.FullName);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenNationalityDoesNotExist()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.Nationalities.GetByIdAsync(1))
            .ReturnsAsync((Nationality?)null);

        // Act
        var result = await _controller.Update(1, SampleUpsertDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContentResult_WhenSuccessful()
    {
        // Arrange
        var nationality = SampleNationality;

        _mockUnitOfWork.Setup(u => u.Nationalities.GetByIdAsync(1)).ReturnsAsync(nationality);
        _mockUnitOfWork.Setup(u => u.Nationalities.Delete(nationality));
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNationalityDoesNotExist()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.Nationalities.GetByIdAsync(1))
            .ReturnsAsync((Nationality?)null);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
