using System.Threading.Tasks;


namespace ApiShortUrl.Services.UrlService
{

    public interface IUrlService
    {
        UrlModel CreateUrl(string originalUrl);

        UrlModel? GetUrl(string shortId);

    }

}
