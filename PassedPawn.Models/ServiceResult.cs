namespace PassedPawn.Models;

public class ServiceResult<T>
{
    public T? Data { get; init; }
    public IEnumerable<string> Errors { get; init; } = [];
    public bool Success => !Errors.Any();
}