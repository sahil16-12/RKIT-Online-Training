using Microsoft.AspNetCore.Http;
using NLog;
using System.Diagnostics;

namespace backend.Middleware
{
    /// <summary>
    /// Logs incoming and outgoing request details with timing.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        #region Private Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            string method = context.Request.Method;
            string path = context.Request.Path.Value ?? string.Empty;
            string contentType = string.IsNullOrWhiteSpace(context.Request.ContentType) ? "n/a" : context.Request.ContentType;
            string correlationId = context.TraceIdentifier;

            Logger.Info("Incoming request method={Method} path={Path} type={ContentType} correlationId={CorrelationId}", method, path, contentType, correlationId);

            Stopwatch stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            int statusCode = context.Response.StatusCode;
            NLog.LogLevel level = statusCode >= 500
                ? NLog.LogLevel.Error
                : statusCode >= 400
                    ? NLog.LogLevel.Warn
                    : NLog.LogLevel.Info;

            Logger.Log(level,
                "Outgoing response method={Method} path={Path} status={StatusCode} durationMs={DurationMs} correlationId={CorrelationId}",
                method,
                path,
                statusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }

        #endregion
    }
}
