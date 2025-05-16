using Server.Middlewares;

namespace Server.Extensions
{
    public static class GzipDecompressionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGzipRequestDecompression(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GzipDecompressionMiddleware>();
        }
    }
}
