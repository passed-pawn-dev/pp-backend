using System.Text.Json;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PassedPawn.BusinessLogic.Services.Contracts;
using PassedPawn.Models.DTOs;

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

    public async Task<DeletionResult> DeleteVideoAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Video
        };
        return await _cloudinary.DestroyAsync(deletionParams);
    }
    
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Image
        };
        return await _cloudinary.DestroyAsync(deletionParams);
    }

    public CloudinarySecureUrl GetUploadSignature(string folderName, string fileType, string accessType, string invalidate = "false")
    {
        var timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        var now = DateTime.UtcNow;
    
        var accessControl = accessType == "private" 
            ? JsonSerializer.Serialize(new[] { new { access_type = "anonymous", start = now.ToString("o"), end = now.ToString("o") } })
            : JsonSerializer.Serialize(new[] { new { access_type = "anonymous" } });
    
        var parameters = new SortedDictionary<string, object>
        {
            { "timestamp", timestamp },
            { "folder", folderName },
            { "resource_type", fileType },
            { "type", accessType },
            { "access_control", accessControl },
            { "invalidate", invalidate }
        };
    
        return new CloudinarySecureUrl
        {
            Signature = _cloudinary.Api.SignParameters(parameters),
            Timestamp = timestamp.ToString(),
            ApiKey = _cloudinary.Api.Account.ApiKey,
            CloudName = _cloudinary.Api.Account.Cloud,
            Folder = folderName,
            ResourceType = fileType,
            AccessType = accessType,
            AccessControl = accessControl,
            Invalidate = invalidate
        };
    }

    public bool IsUrlValid(string url)
    {
        var expectedPrefix = $"https://res.cloudinary.com/{_cloudinary.Api.Account.Cloud}/";
        return url.StartsWith(expectedPrefix);
    }
}
