namespace PassedPawn.Models.DTOs.User.Coach;

public class CoachUpsertDto : UserUpsertDto
{
    public string? DetailedDescription { get; init; }
    public string? ShortDescription { get; init; }
}
