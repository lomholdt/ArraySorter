using Microsoft.AspNetCore.Builder;

namespace ArraySorter.Api.Middleware
{
    public static class AmIUpMiddlewareExtensions
    {
        public static IApplicationBuilder UseAmIUp(this IApplicationBuilder builder) =>
            builder.UseMiddleware<AmIUpMiddleware>();
    }
}
