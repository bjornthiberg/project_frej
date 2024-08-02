namespace project_frej.Middleware;

/// <summary>
/// Middleware that checks for a valid API key in the request headers for POST, PUT, PATCH, and DELETE requests.
/// If the API key is missing or invalid, the middleware will return a 401 Unauthorized response.
/// </summary>
/// <param name="next">The next middleware delegate in the pipeline.</param>
/// <param name="logger">The logger instance to use for logging warnings.</param>
public class ApiKeyPostMiddleware(RequestDelegate next, ILogger<ApiKeyPostMiddleware> logger)
{
    private const string API_KEY_HEADER_NAME = "Authorization";

    /// <summary>
    /// Invokes the middleware to check for a valid API key.
    /// </summary>
    /// <param name="context">The HTTP context of the current request.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (!IsMethodRequiringApiKey(context))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
        {
            logger.LogWarning("API Key was not provided.");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var apiKey = configuration.GetValue<string>("ApiKey");

        if (string.IsNullOrEmpty(apiKey))
        {
            logger.LogWarning("API Key configuration missing. Add to appsettings.json.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal server error.");
            return;
        }

        if (!apiKey.Equals(extractedApiKey))
        {
            logger.LogWarning("Unauthorized client.");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await next(context);
    }

    /// <summary>
    /// Determines if the request method requires an API key.
    /// </summary>
    /// <param name="context">The HTTP context of the current request.</param>
    /// <returns>True if the request method is POST, PUT, PATCH, or DELETE; otherwise, false.</returns>
    private static bool IsMethodRequiringApiKey(HttpContext context)
    {
        var methodsRequiringApiKey = new[] { "POST", "PUT", "PATCH", "DELETE" };
        return methodsRequiringApiKey.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase);
    }
}
