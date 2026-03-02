using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using DoctorAppointmentAPI.Filters;

namespace DoctorAppointmentAPI
{
    /// <summary>
    /// Global application class
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application start event handler
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(SwaggerConfig.Register);
        }
    }
}
