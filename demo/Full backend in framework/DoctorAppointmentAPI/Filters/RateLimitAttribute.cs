using DoctorAppointmentAPI.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DoctorAppointmentAPI.Helpers
{
    /// <summary>
    /// Apply to controllers or actions to rate limit based on a key (username/header/IP).
    /// Constructor sets max attempts and window (seconds).
    /// Optionally set KeyProperty (name of property on a request DTO, e.g. "Username") or KeyFromHeader (HTTP header name) or KeyFromClaims.
    /// If neither is provided, the filter will fallback to client IP.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RateLimitAttribute : ActionFilterAttribute
    {
        public int MaxAttempts { get; }
        public int WindowSeconds { get; }

        /// <summary>
        /// If set, the filter will look for a property with this name on any action argument (e.g. requestDto.Username).
        /// </summary>
        public string KeyProperty { get; set; }

        /// <summary>
        /// If set, the filter will read the key directly from an HTTP header (e.g. X-Api-Key).
        /// </summary>
        public string KeyFromHeader { get; set; }

        /// <summary>
        /// If set, the filter will read the key directly from the jwt claims (e.g. X-Api-Key).
        /// </summary>
        public string KeyFromClaim { get; set; }


        public RateLimitAttribute(int maxAttempts = 2, int windowSeconds = 60)
        {
            MaxAttempts = maxAttempts;
            WindowSeconds = windowSeconds;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string key = ResolveKey(actionContext);

            // If user is not findable in any way
            if (key == "anonymous")
            {
                actionContext.Response =
                    actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.Unauthorized,
                        "User not identifiable"
                    );
                return;
            }

            if (RateLimiterService.Instance.IsBlocked(key, MaxAttempts, WindowSeconds, out int remaining))
            {
                HttpResponseMessage resp = actionContext.Request.CreateResponse(
                    (HttpStatusCode)429,
                    $"Too many attempts. Try again in {remaining} seconds.");
                actionContext.Response = resp;
                return;
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // Resolve the same key used in OnActionExecuting
            var key = ResolveKey(actionExecutedContext.ActionContext);

            // If the action returned 401 -> consider this a failed attempt
            // Added StatusCode of OK becuase of spam appointment booking
            if (actionExecutedContext.Response != null &&
                actionExecutedContext.Response.StatusCode == HttpStatusCode.Unauthorized || actionExecutedContext.Response.StatusCode == HttpStatusCode.OK)
            {
                RateLimiterService.Instance.RecordFailedAttempt(key, WindowSeconds);
            }
            else if (actionExecutedContext.Response != null &&
                     actionExecutedContext.Response.IsSuccessStatusCode)
            {
                // on success clear previous failed attempts
                RateLimiterService.Instance.Clear(key);
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// Resolve key from:
        /// 1) header (KeyFromHeader)
        /// 2) property on action args (KeyProperty)
        /// 3) direct action argument named exactly KeyProperty (like "username")
        /// 4) HttpContext.Current.UserHostAddress (client IP)
        /// 5) fallback "anonymous"
        /// </summary>
        private string ResolveKey(HttpActionContext actionContext)
        {
            try
            {
                // 1) header
                if (!string.IsNullOrWhiteSpace(KeyFromHeader))
                {
                    if (actionContext.Request.Headers.Contains(KeyFromHeader))
                        return actionContext.Request.Headers.GetValues(KeyFromHeader).FirstOrDefault();
                }

                // 2) look for a property on any action argument
                if (!string.IsNullOrWhiteSpace(KeyProperty))
                {
                    // first check if an action argument exists with the same name
                    if (actionContext.ActionArguments.TryGetValue(KeyProperty, out object valDirect))
                    {
                        if (valDirect != null)
                        {
                            return valDirect.ToString();
                        }
                    }

                    // else inspect each argument for property KeyProperty
                    foreach (var arg in actionContext.ActionArguments.Values)
                    {
                        if (arg == null) continue;
                        var prop = arg.GetType().GetProperty(KeyProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (prop != null)
                        {
                            var v = prop.GetValue(arg);
                            if (v != null) return v.ToString();
                        }
                    }
                }

                // 3) look for property in the jwt token
                if (!string.IsNullOrWhiteSpace(KeyFromClaim))
                {
                    ClaimsPrincipal principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;
                    if (principal != null)
                    {
                        Claim claim = principal.FindFirst(KeyFromClaim);
                        if (claim != null && !string.IsNullOrWhiteSpace(claim.Value))
                        {
                            return claim.Value;
                        }
                    }
                }

                // 4) fallback to IP
                if (HttpContext.Current?.Request != null)
                {
                    try
                    {
                        var ip = HttpContext.Current.Request.UserHostAddress;
                        if (!string.IsNullOrWhiteSpace(ip))
                            return ip;
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
            catch
            {
                // Do NOT break request flow.
                // Just fallback safely.
            }

            return "anonymous";
        }
    }
}
