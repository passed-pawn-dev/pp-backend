namespace PassedPawn.Models.DTOs.Puzzle;

public class PuzzleDto
{
    public int Id { get; set; }
    
    public required string Fen { get; set; }
    
    public int CoachId { get; set; }
    
}