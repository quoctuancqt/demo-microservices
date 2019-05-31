namespace CoreDistributedCache
{
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;

    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key) where T : class
        {
            return BytesToObj<T>(_cache.Get(key));
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default) where T : class
        {
            return BytesToObj<T>(await _cache.GetAsync(key, token));
        }

        public void Refresh(string key)
        {
            _cache.Refresh(key);
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            await _cache.RefreshAsync(key, token);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            await _cache.RemoveAsync(key, token);
        }

        public void Set<T>(string key, T value, DistributedCacheEntryOptions options = null)
            where T : class
        {
            options = options ?? GetDefaultCachOptions();

            _cache.Set(key, ObjToBytes(value), options);
        }

        public async Task SetAsync<T>(string key, T value,
            DistributedCacheEntryOptions options = null,
            CancellationToken token = default) where T : class
        {
            options = options == null ? GetDefaultCachOptions() : options;

            await _cache.SetAsync(key, ObjToBytes(value), options, token);
        }

        private T BytesToObj<T>(byte[] bytes) where T : class
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), jsonSettings);
        }

        private byte[] ObjToBytes<T>(T value) where T : class
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, jsonSettings));
        }

        private DistributedCacheEntryOptions GetDefaultCachOptions()
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(8));

            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30);

            return options;
        }
    }
}
