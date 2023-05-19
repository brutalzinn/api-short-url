using System;
using ApiShortUrl.Models;
using ApiShortUrl.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

public class UrlModel
{
    public string Id { get; set; }
    public string MD5 { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortUrl { get; set; }
    public string ShortId { get; set; }
    public UrlModel(string mD5, string originalUrl)
    {
        Id = Guid.NewGuid().ToString();
        MD5 = mD5;
        OriginalUrl = originalUrl;
        ShortId = Id.Substring(0, 5);
    }


    public void CreateUrlSchema(HttpContext httpContext)
    {
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{ShortId}";
    }

    public void Validate()
    {
        var isValidUrl = Uri.TryCreate(OriginalUrl, UriKind.Absolute, out var result);
        if (isValidUrl == false)
        {
            throw new CustomException(TypeException.BUSINESS_LOGIC, "You url is invalid.");
        }
    }

}