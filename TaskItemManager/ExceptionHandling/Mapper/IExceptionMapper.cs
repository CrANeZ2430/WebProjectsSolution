using Microsoft.AspNetCore.Mvc;

namespace TaskItemManager.ExceptionHandling.Mapper;

public interface IExceptionMapper
{
    Microsoft.AspNetCore.Mvc.ProblemDetails MapException(HttpContext httpContext, Exception exception);
}
