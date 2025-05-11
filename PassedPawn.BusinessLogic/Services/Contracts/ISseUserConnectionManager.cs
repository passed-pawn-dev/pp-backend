using Microsoft.AspNetCore.Http;

namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface ISseUserConnectionManager
{
    void AddConnection(int userId, HttpResponse response);
    void RemoveConnection(int userId);
    Task SendEventAsync(int userId, string eventData);
}
