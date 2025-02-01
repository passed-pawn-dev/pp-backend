namespace PassedPawn.DataAccess.Entities;

public class Puzzle : IEntity
{
    public int Id { get; set; }
    
    public required string Fen { get; set; }
    
    public int CoachId { get; set; }
    
    public Coach? Coach { get; set; }
    
    public required string Solution { get; set; }

    public List<Student> Students { get; set; } = [];
}