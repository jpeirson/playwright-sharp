using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PlaywrightSharp.TestServer
{
    internal class SimpleCompressionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SimpleServer _server;

        public SimpleCompressionMiddleware(RequestDelegate next, SimpleServer server)
        {
            _next = next;
            _server = server;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_server.GzipRoutes.Contains(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var response = context.Response.Body;
            var bodyWrapperStream = new MemoryStream();
            context.Response.Body = bodyWrapperStream;

            await _next(context);
            using (var stream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(stream, CompressionMode.Compress, true))
                {
                    bodyWrapperStream.Position = 0;
                    bodyWrapperStream.CopyTo(compressionStream);
                }

                context.Response.Headers["Content-Encoding"] = "gzip";
                context.Response.Headers["Content-Length"] = stream.Length.ToString();
                stream.Position = 0;
                await stream.CopyToAsync(response);
                context.Response.Body = response;
            }
        }
    }
}
