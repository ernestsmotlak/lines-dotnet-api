
public class ApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "X-API-KEY";
    private const string ApiKey = "secret-key";

    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey) || providedApiKey != ApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: API key is missing or invalid.");
            return;
        }

        await _next(context);
    }
}
