using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICloudinaryService
{
    Task<UploadResult> UploadAsync(IFormFile file);
    Task<DeletionResult> DeleteAsync(string publicId, ResourceType resourceType = ResourceType.Video);
}