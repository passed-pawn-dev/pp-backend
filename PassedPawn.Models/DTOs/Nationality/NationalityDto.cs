namespace PassedPawn.Models.DTOs.Nationality;

public class NationalityDto
{
    public int Id { get; init; }
    public required string FullName { get; init; }
    public required string ShortName { get; init; }
}
