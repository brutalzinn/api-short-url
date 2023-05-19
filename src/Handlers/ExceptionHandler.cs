using System;
using ApiShortUrl.Models;
using ApiShortUrl.Models.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace ApiShortUrl.Handlers
{
    public static class ExceptionHandler
    {
        public static void AddCustomExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(builder =>
            builder.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerPathFeature?.Error is CustomException customException)
                {
                    context.Response.StatusCode = customException.StatusCode;

                    switch (customException.Type)
                    {
                        case TypeException.AUTHORIZATION:
                            context.Response.Redirect("static/error.html");
                            break;
                        default:
                            await context.Response.WriteAsJsonAsync(customException.GetResponse());
                            break;

                    }
                }
            }));
        }

    }
}
