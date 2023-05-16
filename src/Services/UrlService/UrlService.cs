
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApiShortUrl.Models;
using ApiShortUrl.Models.Exceptions;
using ApiShortUrl.Services.Redis;

namespace ApiShortUrl.Services.UrlService
{

    public class UrlService : IUrlService
    {
        private IRedisService _redisService { get; set; }

        public UrlService(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public UrlModel CreateUrl(string originalUrl)
        {
            var md5 = CreateMD5Hash(originalUrl);
            var urlModel = new UrlModel(md5, originalUrl);
            _redisService.Set(urlModel.ShortId, urlModel);
            return urlModel;
        }

        public UrlModel? GetUrl(string shortId)
        {
            var urlExists = _redisService.Exists(shortId);
            if (urlExists == false)
            {
                return null;
            }
            var urlModel = _redisService.Get<UrlModel>(shortId);
            return urlModel;
        }

        private string CreateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
