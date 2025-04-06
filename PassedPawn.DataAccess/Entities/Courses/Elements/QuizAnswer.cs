namespace PassedPawn.DataAccess.Entities.Courses.Elements;

public class QuizAnswer : IEntity
{
    public int Id { get; set; }
    
    public string Text { get; set; }
    
    public string LastMove;
}