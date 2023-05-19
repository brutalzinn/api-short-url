using System.Threading.Tasks;


namespace ApiShortUrl.Services.UrlService
{

    public interface IUrlService
    {
        UrlModel CreateUrl(string originalUrl);

        UrlModel? GetByShortId(string shortId);

        UrlModel? GetByMd5(string shortId);


    }

}
