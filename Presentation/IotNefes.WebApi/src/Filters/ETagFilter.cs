using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IotNefes.WebApi.src.Filters
{
    public class ETagFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            if (request.Method != HttpMethods.Get)
            {
                await next();
                return;
            }

            if (context.Result is ObjectResult objectResult && objectResult.Value is not null)
            {
                var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(objectResult.Value);
                var hash = SHA256.HashData(jsonBytes);
                var etag = $"\"{Convert.ToBase64String(hash)}\"";

                context.HttpContext.Response.Headers.ETag = etag;

                if (request.Headers.IfNoneMatch.ToString() == etag)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }

            await next();
        }
    }
}