using project_frej.Models;

namespace project_frej.Services;

public interface IWebSocketHandler
{
    /// <summary>
    /// Handles an incoming WebSocket request.
    /// </summary>
    /// <param name="context">The HTTP context containing the WebSocket request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleWebSocketAsync(HttpContext context);
    /// <summary>
    /// Notifies connected WebSocket clients of a new sensor reading.
    /// </summary>
    /// <param name="sensorReading">The sensor reading to send to clients.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task NotifyClients(SensorReading sensorReading);
}
