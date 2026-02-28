using NLog;

namespace backend.Middleware
{
    /// <summary>
    /// Ensures each request has a correlation id and propagates it in response and logging scope.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        #region Constants

        private const string CorrelationIdHeader = "X-Correlation-ID";

        #endregion

        #region Private Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out Microsoft.Extensions.Primitives.StringValues value)
                && !string.IsNullOrWhiteSpace(value.FirstOrDefault())
                    ? value.First()!
                    : Guid.NewGuid().ToString("N");

            context.TraceIdentifier = correlationId;

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });

            using (ScopeContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }

        #endregion
    }
}
