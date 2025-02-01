namespace PassedPawn.DataAccess.Entities;

public class Student : User
{
    public List<Puzzle> Puzzles { get; set; } = [];
}