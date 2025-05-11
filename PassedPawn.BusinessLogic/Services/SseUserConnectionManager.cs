using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PassedPawn.BusinessLogic.Services.Contracts;

namespace PassedPawn.BusinessLogic.Services;

public class SseUserConnectionManager : ISseUserConnectionManager
{
    private readonly ConcurrentDictionary<int, HttpResponse> _connections = new();

    public void AddConnection(int userId, HttpResponse response)
    {
        _connections[userId] = response;
    }

    public void RemoveConnection(int userId)
    {
        _connections.TryRemove(userId, out _);
    }

    public async Task SendEventAsync(int userId, string eventData)
    {
        if (_connections.TryGetValue(userId, out var response))
        {
            if (!response.HttpContext.RequestAborted.IsCancellationRequested)
            {
                var message = $"data: {eventData}\n\n";
                await response.WriteAsync(message);
                await response.Body.FlushAsync();
            }
        }
    }
}
