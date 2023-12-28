using System.ComponentModel.DataAnnotations;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public record ErrorObjectResult(string Message);
public class ExceptionsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case EntityNotFoundException:
                context.Result = new NotFoundObjectResult(new ErrorObjectResult(context.Exception.Message));
                break;
            
            case ValidationException:
            case ApplicationValidationException:
                context.Result = new BadRequestObjectResult(new ErrorObjectResult(context.Exception.Message));
                break;
        }
    }
}