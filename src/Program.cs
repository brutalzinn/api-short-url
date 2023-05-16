using ApiShortUrl;
using ApiShortUrl.Handlers;
using ApiShortUrl.Routes.Geradores;
using Microsoft.AspNetCore.Builder;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        DependencyInjection.Inject(builder.Services);
        var app = builder.Build();
        app.UseCors("corsapp");

        app.UseAuthentication();
        app.UseAuthorization();
        app.AddCustomExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI();

        UrlRoute.Create(app);

        app.Run();
    }
}

