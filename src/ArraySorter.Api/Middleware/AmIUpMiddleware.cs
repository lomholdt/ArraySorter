using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ArraySorter.Api.Middleware
{
    public class AmIUpMiddleware
    {
        private readonly RequestDelegate _next;

        public AmIUpMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/")
            {
                await context.Response.WriteAsync("ok");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
