using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

using project_frej.Models;

namespace project_frej.Services;

/// <summary>
/// Handles WebSocket connections and notifications for sensor readings.
/// </summary>
/// <param name="logger">The logger instance for logging information and errors.</param>
public class WebSocketHandler(ILogger<WebSocketManager> logger)
{
    private static readonly List<WebSocket> _sockets = [];

    /// <summary>
    /// Handles an incoming WebSocket request.
    /// </summary>
    /// <param name="context">The HTTP context containing the WebSocket request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleWebSocketAsync(HttpContext context)
    {
        logger.LogInformation("WebSocket connection established");
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _sockets.Add(webSocket);

            await KeepConnectionOpen(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    /// <summary>
    /// Keeps the WebSocket connection open and listens for close messages.
    /// </summary>
    /// <param name="webSocket">The WebSocket to keep open.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Notifies connected WebSocket clients of a new sensor reading.
    /// </summary>
    /// <param name="sensorReading">The sensor reading to send to clients.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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
