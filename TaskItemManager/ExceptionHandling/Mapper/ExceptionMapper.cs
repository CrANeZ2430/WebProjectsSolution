using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using TaskItemManager.ExceptionHandling.Exceptions;

namespace TaskItemManager.ExceptionHandling.Mapper;

public class ExceptionMapper : IExceptionMapper
{
    public ProblemDetails MapException(
        HttpContext httpContext,
        Exception exception)
    {
        int statusCode;
        string title;
        var errors = new Dictionary<string, string[]>();

        switch(exception)
        {
            case ValidationException e:
                statusCode = 400;
                title = "Validation failed";
                errors = e.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());
                break;
            case BadRequestException:
                statusCode = 400;
                title = "Error caused by request";
                break;
            case ForbiddenException:
                statusCode = 403;
                title = "Cannot access resource";
                break;
            case NotFoundException:
                statusCode = 404;
                title = "Resource cannot be found";
                break;
            case InternalServerErrorException:
            default:
                statusCode = 500;
                title = "Internal server problem";
                break;
        }

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Status = statusCode,
            Title = title
        };

        if (errors.Count() == 0)
            problemDetails.Detail = exception.Message;
        else
            problemDetails.Extensions.Add("errors", errors);

        return problemDetails;
    }
}
