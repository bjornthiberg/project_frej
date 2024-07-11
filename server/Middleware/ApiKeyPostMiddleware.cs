namespace project_frej.Middleware
{
    public class ApiKeyPostMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyPostMiddleware> _logger;
        private const string API_KEY_HEADER_NAME = "Authorization";

        public ApiKeyPostMiddleware(RequestDelegate next, ILogger<ApiKeyPostMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (!IsPostRequest(context) && !IsPutRequest(context) && !IsPatchRequest(context) && !IsDeleteRequest(context))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                _logger.LogWarning("API Key was not provided.");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            var apiKey = configuration.GetValue<string>("ApiKey");

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("API Key configuration missing. Add to appsettings.json.");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal server error.");
                return;
            }

            if (!apiKey.Equals(extractedApiKey))
            {
                _logger.LogWarning("Unauthorized client.");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            await _next(context);
        }

        private static bool IsPostRequest(HttpContext context)
        {
            return context.Request.Method.Equals("POST");
        }

        private static bool IsPutRequest(HttpContext context)
        {
            return context.Request.Method.Equals("PUT");
        }

        private static bool IsPatchRequest(HttpContext context)
        {
            return context.Request.Method.Equals("PATCH");
        }

        private static bool IsDeleteRequest(HttpContext context)
        {
            return context.Request.Method.Equals("DELETE");
        }

    }

}
