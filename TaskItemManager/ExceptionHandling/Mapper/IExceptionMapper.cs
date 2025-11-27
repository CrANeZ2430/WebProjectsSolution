using Microsoft.AspNetCore.Mvc;

namespace TaskItemManager.ExceptionHandling.Mapper;

public interface IExceptionMapper
{
    ProblemDetails MapException(HttpContext httpContext, Exception exception);
}
