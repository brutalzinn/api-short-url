﻿using Cronos;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ApiShortUrl.Models.Settings;

namespace ApiShortUrl.Services.Redis
{
    public class RedisService : IRedisService
    {
        private IDistributedCache _redisCache;
        private ApiConfig _apiConfig;

        public RedisService(IDistributedCache redisCache, Microsoft.Extensions.Options.IOptions<ApiConfig> apiConfig)
        {
            _redisCache = redisCache;
            _apiConfig = apiConfig.Value;

        }
        public T Get<T>(string chave)
        {
            var value = _redisCache.GetString(chave);
            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public void Set<T>(string chave, T valor)
        {
            _redisCache.SetString(chave, JsonSerializer.Serialize(valor), GetExpirationTime());
        }

        public bool Clear(string chave)
        {
            _redisCache.Remove(chave);
            return default;
        }

        public bool Exists(string chave)
        {
            return _redisCache.Get(chave) != null;
        }

        /// <summary>
        /// Usar apenas para cache de listas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chave"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public void ItemAdd<T>(string chave, T valor)
        {
            var value = _redisCache.GetString(chave);
            var lista = new List<T>();
            if (value != null)
            {
                lista = JsonSerializer.Deserialize<List<T>>(value);
                lista.Add(valor);
                _redisCache.SetString(chave, JsonSerializer.Serialize(lista), GetExpirationTime());
            }
            lista.Add(valor);
            _redisCache.SetString(chave, JsonSerializer.Serialize(lista), GetExpirationTime());
        }


        /// <summary>
        /// Usar apenas para cache de listas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chave"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public void ItemRemove<T>(string chave, int index)
        {
            var value = _redisCache.GetString(chave);
            if (value != null)
            {
                return;
            }
            var lista = JsonSerializer.Deserialize<List<T>>(value);
            lista.RemoveAt(index);
            _redisCache.SetString(chave, JsonSerializer.Serialize(lista));

        }
        ///temporary. now i found the problem with my expiration time.
        private DistributedCacheEntryOptions GetExpirationTime()
        {
            var cronParsed = CronExpression.Parse(_apiConfig.CacheConfig.ExpireEvery);
            var distributedCacheEntry = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = cronParsed.GetNextOccurrence(DateTime.UtcNow),
            };
            return distributedCacheEntry;
        }

    }
}
