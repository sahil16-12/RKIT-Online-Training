using System.Text.Json;
using backend.DTOs;
using backend.Exceptions;
using NLog;

namespace backend.Middleware
{
    /// <summary>
    /// Handles unhandled exceptions globally and returns a consistent API error response.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        #region Private Fields

        /// <summary>
        /// Represents the next middleware delegate.
        /// </summary>
        private readonly RequestDelegate _next;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware delegate.</param>
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes middleware logic for each request.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException appException)
            {
                NLog.LogLevel level = appException.StatusCode >= StatusCodes.Status500InternalServerError
                    ? NLog.LogLevel.Error
                    : NLog.LogLevel.Warn;

                Logger.Log(level,
                    appException,
                    "App exception status={StatusCode} method={Method} path={Path} correlationId={CorrelationId}",
                    appException.StatusCode,
                    context.Request.Method,
                    context.Request.Path,
                    context.TraceIdentifier);

                await WriteErrorResponseAsync(context, appException.StatusCode, appException.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex,
                    "Unhandled exception status=500 method={Method} path={Path} correlationId={CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.TraceIdentifier);
                await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError, "Server error.");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes standardized error response payload.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="message">The error message.</param>
        private static async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            ErrorResponse errorResponse = new ErrorResponse
            {
                Message = string.IsNullOrWhiteSpace(message) ? "Request failed." : message
            };

            string payload = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(payload);
        }

        #endregion
    }
}
