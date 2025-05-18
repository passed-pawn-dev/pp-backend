using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PassedPawn.Models.Validators;

public class ImageFileAttribute : ValidationAttribute
{
    private readonly string[] _permittedMimeTypes = ["image/jpeg", "image/png", "image/webp", "image/tiff"];
    private readonly long _maxFileSizeBytes;

    public ImageFileAttribute(long maxFileSizeBytesMb = 5)
    {
        _maxFileSizeBytes = maxFileSizeBytesMb * 1024 * 1024;
        ErrorMessage = $"File must be an image and no larger than {maxFileSizeBytesMb}MB.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
            return new ValidationResult("No file was provided.");

        if (!_permittedMimeTypes.Contains(file.ContentType))
            return new ValidationResult("Only image files (JPEG, PNG, WebP, TIFF) are allowed.");

        return file.Length > _maxFileSizeBytes ?
            new ValidationResult($"File must be no larger than {_maxFileSizeBytes / (1024 * 1024)}MB.") :
            ValidationResult.Success;
    }
}
