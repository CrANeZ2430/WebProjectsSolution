﻿using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using TaskItemManager.ExceptionHandling.Handlers;
using TaskItemManager.ExceptionHandling.Mapper;

namespace TaskItemManager.ExceptionHandling;

public static class ExceptionHandlingMiddlewareRegistration
{
    public static void RegisterExceptionHandling(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddExceptionHandler<CustomExceptionHandler>();
        serviceCollection.AddSingleton<IExceptionMapper, ExceptionMapper>();
        serviceCollection.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);


                if (context.Exception.StackTrace is not null)
                    context.ProblemDetails.Extensions.Add(
                        "stackTrace",
                        context.Exception.StackTrace
                        .Split(
                            new[] { "\r\n", "\n" },
                            StringSplitOptions.TrimEntries));
            };
        });
    }
}
