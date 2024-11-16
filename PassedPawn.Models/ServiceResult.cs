#nullable disable

namespace PassedPawn.Models;

public class ServiceResult<T>
{
    public T Data { get; init; }
    public IEnumerable<string> Errors { get; init; } = [];
    public bool IsSuccess => !Errors.Any();

    public static ServiceResult<T> Success(T data)
        => new()
        {
            Data = data
        };
    
    public static ServiceResult<T> Failure(IEnumerable<string> errors)
        => new()
        {
            Errors = errors
        };
}
