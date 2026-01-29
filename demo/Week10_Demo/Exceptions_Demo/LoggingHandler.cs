using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web;

namespace Exceptions_Demo
{
    /// <summary>
    /// DelegatingHandler used to log request info and catch early pipeline exceptions.
    /// Demonstrates where logging and metrics are commonly captured.
    /// </summary>
    public class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Simple console logging for demo
            System.Console.WriteLine("Request: " + request.Method + " " + request.RequestUri);

            try
            {
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                System.Console.WriteLine("Response: " + (int)response.StatusCode + " " + response.ReasonPhrase);
                return response;
            }
            catch (Exception ex)
            {
                // Log exception and rethrow so filters/handlers can handle it
                System.Console.WriteLine("Unhandled exception in pipeline: " + ex.Message);
                throw;
            }
        }
    }
}