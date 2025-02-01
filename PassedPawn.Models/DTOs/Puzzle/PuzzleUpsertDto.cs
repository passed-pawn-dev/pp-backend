using System.ComponentModel.DataAnnotations;

namespace PassedPawn.Models.DTOs.Puzzle;

public class PuzzleUpsertDto
{
    [Required]
    public string Fen { get; set; } = string.Empty;
    
    [Required]
    public string Solution { get; set; } = string.Empty;
}