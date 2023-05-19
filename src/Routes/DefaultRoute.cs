
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ApiShortUrl.Routes.Geradores
{
    public static class DefaultRoute
    {
        public static void Create(this WebApplication app)
        {
            app.MapGet("/howto",
            (HttpContext httpContext) =>
            {
                httpContext.Response.Redirect("static/index.html");
            });

            app.MapGet("/notfound",
            (HttpContext httpContext) =>
            {
                httpContext.Response.Redirect("static/error.html");
            });
        }
    }
}




