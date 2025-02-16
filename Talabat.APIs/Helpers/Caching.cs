using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs
{
    public class CachingAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _seconds;

        public CachingAttribute(int seconds)
        {
            _seconds = seconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var cachedResponse = await cachedService.GetCachedResponse(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var content = new ContentResult()
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = content;
                return;
            }

            var executedEndPoint = await next.Invoke();
            if (executedEndPoint.Result is OkObjectResult result)
            {
                await cachedService.CacheResponseAsync(cacheKey, result.Value, TimeSpan.FromSeconds(_seconds));
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var stringkey = new StringBuilder();
            stringkey.Append(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                stringkey.Append($"|{key}-{value}");
            }
            return stringkey.ToString();
        }
    }
}