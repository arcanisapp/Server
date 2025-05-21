using StackExchange.Redis;

namespace Server.Data
{
    public interface IRedisKeyValueStore<TValue>
    {
        Task<bool> AddAsync(string key, TValue value, TimeSpan? expiry = null, bool onlyIfNotExists = false);
        Task<TValue?> GetAsync(string key);
        Task<bool> RemoveAsync(string key);
        Task<bool> RefreshTtlAsync(string key, TimeSpan? expiry = null);
    }

    public abstract class RedisKeyValueStoreBase<TValue> : IRedisKeyValueStore<TValue>
    {
        protected readonly IDatabase _db;

        protected RedisKeyValueStoreBase(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        protected abstract RedisValue Serialize(TValue value);
        protected abstract TValue? Deserialize(RedisValue value);

        public async Task<bool> AddAsync(string key, TValue value, TimeSpan? expiry = null, bool onlyIfNotExists = false)
        {
            var val = Serialize(value);
            return await _db.StringSetAsync(
                key,
                val,
                expiry, 
                onlyIfNotExists ? When.NotExists : When.Always);
        }

        public async Task<TValue?> GetAsync(string key)
        {
            var val = await _db.StringGetAsync(key);
            if (val.IsNullOrEmpty)
                return default;

            return Deserialize(val);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }

        public async Task<bool> RefreshTtlAsync(string key, TimeSpan? expiry = null)
        {
            if (expiry.HasValue)
                return await _db.KeyExpireAsync(key, expiry);
            else
                return await _db.KeyPersistAsync(key); 
        }
    }
}
