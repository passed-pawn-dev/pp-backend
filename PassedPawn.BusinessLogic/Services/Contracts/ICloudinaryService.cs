using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICloudinaryService
{
    Task<UploadResult> UploadVideoAsync(IFormFile file);
    Task<UploadResult> UploadPhotoAsync(IFormFile file);
    Task<DeletionResult> DeleteVideoAsync(string publicId);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}