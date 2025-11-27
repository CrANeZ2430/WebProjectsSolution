using TaskItemManager.ExceptionHandling.Handlers;
using TaskItemManager.ExceptionHandling.Mapper;
using TaskItemManager.ExceptionHandling.ProblemDetailsService;
namespace TaskItemManager.ExceptionHandling;

public static class ExceptionHandlingMiddlewareRegistration
{
    public static void RegisterExceptionHandling(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddExceptionHandler<CustomExceptionHandler>();
        serviceCollection.AddSingleton<IExceptionMapper, ExceptionMapper>();
        serviceCollection.AddSingleton<IProblemDetailsService, ProblemDetailsWriter>();
        //serviceCollection.AddProblemDetails(options =>
        //{
        //    options.CustomizeProblemDetails = context =>
        //    {
        //        context.ProblemDetails.Instance =
        //            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        //        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        //        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        //        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);

        //        context.ProblemDetails.Extensions.Add(
        //            "stackTrace",
        //            context.Exception.StackTrace
        //                .Split(
        //                    ["\r\n", "\n"],
        //                    StringSplitOptions.TrimEntries));
        //    };
        //});
    }
}
