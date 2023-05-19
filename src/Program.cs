using ApiShortUrl;
using ApiShortUrl.Handlers;
using ApiShortUrl.Routes.Geradores;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        DependencyInjection.Inject(builder.Services);
        var app = builder.Build();
        app.UseCors("corsapp");
        app.UseStaticFiles();
        app.AddCustomExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI();

        UrlRoute.Create(app);
        DefaultRoute.Create(app);
        app.Run();
    }
}

