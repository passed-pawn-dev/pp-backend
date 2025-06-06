using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using PassedPawn.Models.DTOs;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ICloudinaryService
{
    Task<DeletionResult> DeleteVideoAsync(string publicId);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
    CloudinarySecureUrl GetUploadSignature(string folderName, string fileType, string accessType, string invalidate = "false");
    string GetDownloadUrl(string assetPublicId, string fileType, string fileFormat, int validForSeconds = 120, bool downloadAsAttachment = false);
    bool IsUrlValid(string url);
}   