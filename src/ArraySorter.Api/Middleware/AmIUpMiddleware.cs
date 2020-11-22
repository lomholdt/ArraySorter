using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;

namespace ArraySorter.Api.Middleware
{
    public class AmIUpMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly byte[] _body = Encoding.UTF8.GetBytes("ok");

        public AmIUpMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/")
            {
                await context.Response.Body.WriteAsync(_body);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
