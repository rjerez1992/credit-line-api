using CreditLineAPI.Models;
using CreditLineAPI.Services;

namespace CreditLineAPI.Middlewares
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private Dictionary<string, List<DateTime>> _previousAcceptedRequests;
        private Dictionary<string, DateTime> _previousRejectedRequest;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
            _previousAcceptedRequests = new Dictionary<string, List<DateTime>>();
            _previousRejectedRequest = new Dictionary<string, DateTime>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            CreditLineApplication application = context.Request.ReadFromJsonAsync<CreditLineApplication>().Result;
            context.Request.Body.Position = 0;

            ICreditLineApplicationService _creditLineApplicationService = context.RequestServices.GetService<ICreditLineApplicationService>();
            application = _creditLineApplicationService.FindPrevious(application.Id);

            if (application != null)
            {
                if (application.IsAccepted && HasReachedMaxRequestsInLastNMinutes(application.Id, 3, 2))
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync(String.Empty);
                    return;
                }
                else if (!application.IsAccepted && HasRequestInLastNSeconds(application.Id, 30)) {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync(String.Empty);
                    return;
                }

            }
            await _next(context);
        }

        private bool HasRequestInLastNSeconds(string applicationId, int seconds) {
            if (_previousRejectedRequest.ContainsKey(applicationId))
            {
                if ((DateTime.Now - _previousRejectedRequest[applicationId]).TotalSeconds < seconds)
                {
                    return true;
                }
                else
                    _previousRejectedRequest[applicationId] = DateTime.Now;
            }
            else
                _previousRejectedRequest.Add(applicationId, DateTime.Now);
            return false;
        }

        private bool HasReachedMaxRequestsInLastNMinutes(string applicationId, int maxRequest, int minutes) {
            if (_previousAcceptedRequests.ContainsKey(applicationId))
            {
                _previousAcceptedRequests[applicationId] = _previousAcceptedRequests[applicationId].Where(d => (DateTime.Now - d).TotalMinutes < minutes).ToList();
                if (_previousAcceptedRequests[applicationId].Count() >= maxRequest) {
                    return true;
                }
                _previousAcceptedRequests[applicationId].Add(DateTime.Now);
            }
            else
                _previousAcceptedRequests.Add(applicationId, new List<DateTime>() { DateTime.Now });
            return false;
        }
    }

    public static class RateLimitMiddlewareExtension
    {
        public static IApplicationBuilder UseRateLimitMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<RateLimitMiddleware>();
        }
    }
}
