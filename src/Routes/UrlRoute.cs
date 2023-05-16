using ApiShortUrl.Services.UrlService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ApiShortUrl.Routes.Geradores
{
    public static class UrlRoute
    {
        public static void Create(this WebApplication app)
        {
            app.MapGet("/url/short",
                [Authorize(AuthenticationSchemes = "ApiKey")]
            async ([FromQuery] string url, [FromServices] IUrlService urlService, IHttpContextAccessor httpContextAccessor) =>
                {
                    var result = urlService.CreateUrl(url);
                    var httpContext = httpContextAccessor.HttpContext;
                    result.CreateUrlShort(httpContext);
                    return result;
                });

            app.MapGet("/{shortid}",
             ([FromRoute] string shortid, [FromServices] IUrlService urlService, IHttpContextAccessor httpContextAccessor) =>
            {
                var result = urlService.GetUrl(shortid);
                var httpContext = httpContextAccessor.HttpContext;
                httpContext.Response.Redirect(result?.OriginalUrl ?? "/notfound");
            });

            app.MapGet("/notfound",
            (IHttpContextAccessor httpContextAccessor) =>
            {
                var httpContext = httpContextAccessor.HttpContext;
                httpContext.Response.ContentType = "text/html";
                return httpContext.Response.SendFileAsync("static/error.html");

            });
        }
    }
}
