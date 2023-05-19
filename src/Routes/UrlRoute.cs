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
            app.MapGet("/short",
            [Authorize(AuthenticationSchemes = "ApiKeyByUrl")]
            ([FromQuery] string url, [FromServices] IUrlService urlService, HttpContext httpContext) =>
                {
                    var result = urlService.CreateUrl(url);
                    result.CreateUrlSchema(httpContext);
                    return result;
                });

            app.MapGet("/{shortid}",
             ([FromRoute] string shortid, [FromServices] IUrlService urlService, HttpContext httpContext) =>
            {
                var result = urlService.GetByShortId(shortid);
                var path = result?.OriginalUrl ?? "/notfound";
                httpContext.Response.Redirect(path);
            });

        }
    }
}
