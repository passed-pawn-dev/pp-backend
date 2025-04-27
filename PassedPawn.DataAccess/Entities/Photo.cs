namespace PassedPawn.DataAccess.Entities;

public class Photo : IEntity
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public required string PublicId { get; set; }
}
