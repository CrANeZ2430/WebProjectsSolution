
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace TaskItemManager.ExceptionHandling.ProblemDetails;

public class ProblemDetailsService : IProblemDetailsService
{
    public async ValueTask WriteAsync(ProblemDetailsContext context)
    {
        var response = context.HttpContext.Response;

        response.ContentType = "application/json";

        response.StatusCode = context.ProblemDetails?.Status ?? 500;

        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);

        context.ProblemDetails.Extensions.Add(
            "stackTrace",
            context.Exception.StackTrace
                .Split(
                    ["\r\n", "\n"],
                    StringSplitOptions.TrimEntries));

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await JsonSerializer.SerializeAsync(response.Body, context.ProblemDetails ?? new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = 500,
            Title = "Unknown error"
        }, options);
    }
}
