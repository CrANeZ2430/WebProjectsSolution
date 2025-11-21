using Microsoft.AspNetCore.Diagnostics;
using TaskItemManager.ExceptionHandling.Mapper;

namespace TaskItemManager.ExceptionHandling.Handlers;

public class CustomExceptionHandler(
    IProblemDetailsService problemDetailService,
    ILogger<CustomExceptionHandler> logger, 
    IExceptionMapper mapper) 
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = mapper.MapException(httpContext, exception);

        try
        {
            await problemDetailService.WriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }

        return true;
    }
}
