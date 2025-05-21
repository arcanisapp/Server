using Server.Crypto;
using Server.Services;
using Server.Services.Validation;

namespace Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IHCaptchaService, HCaptchaService>();

            services.AddScoped<IProofOfWorkService, ProofOfWorkService>();

            services.AddScoped<IShakeGenerator, ShakeGenerator>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IDeviceService, DeviceService>();

            services.AddScoped<IContactService, ContactService>();

            services.AddSingleton<IMlDsaKeyVerifier, MlDsaKeyVerifier>();

            services.AddSingleton<ITimestampValidator>(new TimestampValidator(maxSkewSeconds: 30));

            return services;
        }
    }
}
