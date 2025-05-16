using System.IO.Compression;

namespace Server.Middlewares
{
    public class GzipDecompressionMiddleware
    {
        private readonly RequestDelegate _next;

        public GzipDecompressionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Content-Encoding", out var encoding) &&
                encoding.ToString().Contains("gzip", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.Body = new GZipStream(context.Request.Body, CompressionMode.Decompress);
            }

            await _next(context);
        }
    }
}
