using ArraySorter.Api.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ArraySorter.Api.Middleware
{
    public static class HttpContextExtension
    {
        public static async Task SetErrorResponse(this HttpContext context, Exception e, int statusCode)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = statusCode;

                var response = new ErrorResponse(e.GetType().Name, e.Message, statusCode);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
