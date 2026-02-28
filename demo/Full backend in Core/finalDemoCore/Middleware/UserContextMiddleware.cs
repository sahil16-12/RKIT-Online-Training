using System.Security.Claims;
using backend.Exceptions;
using backend.Models;

namespace backend.Middleware
{
    /// <summary>
    /// Extracts authenticated user context from claims and stores it in HttpContext.Items.
    /// </summary>
    public class UserContextMiddleware
    {
        #region Private Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                string? idClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? roleClaim = context.User.FindFirstValue(ClaimTypes.Role);

                bool isUserIdValid = int.TryParse(idClaim, out int userId);
                bool isRoleValid = Enum.TryParse(roleClaim, true, out UserType role);

                if (!isUserIdValid || !isRoleValid)
                {
                    throw new AppException("Invalid authentication token.", StatusCodes.Status401Unauthorized);
                }

                context.Items[CurrentUserContext.ItemKey] = new CurrentUserContext
                {
                    UserId = userId,
                    Role = role
                };
            }

            await _next(context);
        }

        #endregion
    }
}
