using Server.Data.RedisStore;

namespace Server.Extensions
{
    public static class RedisStoreBaseExtensions
    {
        public static IServiceCollection AddRedisStore(this IServiceCollection services)
        {
            services.AddSingleton<ITempIdConnectionStore, TempIdConnectionStore>();
            return services;
        }
    }
}
