namespace PassedPawn.Models.Validators;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class VideoFileAttribute : ValidationAttribute
{
    private readonly string[] _permittedMimeTypes =
    {
        "video/mp4",
        "video/x-msvideo",
        "video/x-matroska",
        "video/quicktime",
        "video/webm",
        "video/x-flv",
        "video/3gpp",
        "video/3gpp2",
        "application/octet-stream"
    };

    private readonly long _maxFileSizeBytes;

    public VideoFileAttribute(long maxFileSizeMb = 100)
    {
        _maxFileSizeBytes = maxFileSizeMb * 1024 * 1024;
        ErrorMessage = $"File must be a supported video format and no larger than {maxFileSizeMb}MB.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
            return new ValidationResult("No file was provided.");

        if (!_permittedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            return new ValidationResult($"Unsupported video format: {file.ContentType}");

        return file.Length > _maxFileSizeBytes ?
            new ValidationResult($"File size exceeds the limit of {_maxFileSizeBytes / (1024 * 1024)}MB.") :
            ValidationResult.Success;
    }
}
