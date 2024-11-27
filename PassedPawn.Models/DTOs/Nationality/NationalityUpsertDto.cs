using System.ComponentModel.DataAnnotations;
using PassedPawn.Models.DTOs.Photo;

namespace PassedPawn.Models.DTOs.Nationality;

public class NationalityUpsertDto
{
    [StringLength(25)] public required string FullName { get; init; }

    [StringLength(25)] public required string ShortName { get; init; }

    public required PhotoUpsertDto Flag { get; init; }
}