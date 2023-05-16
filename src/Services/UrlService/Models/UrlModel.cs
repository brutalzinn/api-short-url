using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

public class UrlModel
{

    public string Id { get; set; }
    public string MD5 { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortUrl { get; set; }
    public string ShortId { get; }
    public UrlModel(string mD5, string originalUrl)
    {
        Id = Guid.NewGuid().ToString();
        MD5 = mD5;
        OriginalUrl = originalUrl;
        ShortId = Id.Substring(0, 5);
    }

    public void CreateUrlShort(HttpContext httpContext)
    {
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{ShortId}";

    }
}