
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
            var urlExists = IsUrlMd5Registred(md5);
            if (urlExists)
            {
                return GetByMd5(md5);
            }
            var urlModel = new UrlModel(md5, originalUrl);
            urlModel.Validate();
            _redisService.Set(md5, urlModel.ShortId);
            _redisService.Set(urlModel.ShortId, urlModel);
            return urlModel;
        }

        public UrlModel? GetByShortId(string shortId)
        {
            var urlExists = _redisService.Exists(shortId);
            if (urlExists == false)
            {
                return null;
            }
            var urlModel = _redisService.Get<UrlModel>(shortId);
            return urlModel;
        }

        public UrlModel GetByMd5(string md5)
        {
            var urlShortId = _redisService.Get<string>(md5);
            var urlModel = _redisService.Get<UrlModel>(urlShortId);
            return urlModel;
        }

        private bool IsUrlMd5Registred(string md5)
        {
            var urlExists = _redisService.Exists(md5);
            return urlExists;
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
