using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.DataAccess.Entities;

public class Coach : User
{
    public string? DetailedDescription { get; init; }

    public string? ShortDescription { get; init; }
    public ICollection<Course> Courses { get; init; } = [];
}