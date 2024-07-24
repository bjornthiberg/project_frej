using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

using project_frej.Models;

namespace project_frej.Services;

public class WebSocketHandler(ILogger<WebSocketManager> logger)
{
    private static readonly List<WebSocket> _sockets = [];

    public async Task HandleWebSocketAsync(HttpContext context)
    {
        logger.LogInformation("WebSocket connection established");
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _sockets.Add(webSocket);

            // Keep the connection open until the client closes it
            await KeepConnectionOpen(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task KeepConnectionOpen(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                _sockets.Remove(webSocket);
            }
        }
    }

    public async Task NotifyClients(SensorReading sensorReading)
    {
        logger.LogInformation("Notifying WebSocket clients of new sensor reading: {SensorReading}", sensorReading);
        var jsonData = JsonSerializer.Serialize(sensorReading);
        var buffer = Encoding.UTF8.GetBytes(jsonData);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var socket in _sockets.ToList())
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                _sockets.Remove(socket);
            }
        }
    }
}
