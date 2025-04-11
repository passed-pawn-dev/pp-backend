using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PassedPawn.BusinessLogic.Services.Contracts;

namespace PassedPawn.BusinessLogic.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var cloudinaryAccount = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(cloudinaryAccount);
    }
    
    public async Task<UploadResult> UploadAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "lesson_videos"
        };

        return await _cloudinary.UploadAsync(uploadParams);
    }

    public async Task<DeletionResult> DeleteAsync(string publicId, ResourceType resourceType = ResourceType.Video)
    {
        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = resourceType
        };
        return await _cloudinary.DestroyAsync(deletionParams);
    }
}
