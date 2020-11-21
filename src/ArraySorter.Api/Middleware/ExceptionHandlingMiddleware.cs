using ArraySorter.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ArraySorter.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                switch (exception)
                {
                    case InvalidStatusException e:
                        await httpContext.SetErrorResponse(e, StatusCodes.Status400BadRequest);
                        break;
                    case EmptyInputException e:
                        await httpContext.SetErrorResponse(e, StatusCodes.Status400BadRequest);
                        break;
                    default:
                        await httpContext.SetErrorResponse(exception, StatusCodes.Status500InternalServerError);
                        break;
                }
            }
        }
    }
}
