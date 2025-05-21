using StackExchange.Redis;

namespace Server.Data.RedisStore
{
    public interface ITempIdConnectionStore
    {
        Task<bool> AddTempIdConnectionAsync(string tempId, string connectionId);
        Task<string?> GetConnectionIdAsync(string tempId);
        Task<bool> RemoveTempIdAsync(string tempId);
    }
    public class TempIdConnectionStore : RedisKeyValueStoreBase<string>, ITempIdConnectionStore
    {
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(1);

        protected override RedisValue Serialize(string value) => value;

        protected override string Deserialize(RedisValue value) => value;

        public TempIdConnectionStore(IConnectionMultiplexer connectionMultiplexer)
            : base(connectionMultiplexer)
        {
        }

        public Task<bool> AddTempIdConnectionAsync(string tempId, string connectionId)
        {
            return AddAsync(tempId, connectionId, DefaultTtl);
        }

        public Task<string?> GetConnectionIdAsync(string tempId)
        {
            return GetAsync(tempId);
        }

        public Task<bool> RemoveTempIdAsync(string tempId)
        {
            return RemoveAsync(tempId);
        }
    }
}
