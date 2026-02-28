using backend.Exceptions;
using backend.Models;

namespace backend.Extensions
{
    /// <summary>
    /// Provides extension methods for reading strongly typed values from HttpContext.
    /// </summary>
    public static class HttpContextExtensions
    {
        #region Public Methods

        /// <summary>
        /// Gets authenticated user context from HttpContext items.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>The current user context.</returns>
        public static CurrentUserContext GetCurrentUserContext(this HttpContext httpContext)
        {
            if (httpContext.Items.TryGetValue(CurrentUserContext.ItemKey, out object? value)
                && value is CurrentUserContext currentUserContext)
            {
                return currentUserContext;
            }

            throw new AppException("Invalid authentication token.", StatusCodes.Status401Unauthorized);
        }

        #endregion
    }
}
