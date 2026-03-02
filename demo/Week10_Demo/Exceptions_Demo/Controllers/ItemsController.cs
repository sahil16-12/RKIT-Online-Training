using Exceptions_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Exceptions_Demo.Controllers
{
    /// <summary>
    /// Demo controller showing three patterns:
    /// 1) Let action throw -> handled by filter/global handler.
    /// 2) Local try/catch to return custom response.
    /// 3) Using service that translates exceptions.
    /// </summary>
    [RoutePrefix("api/items")]
    public class ItemsController : ApiController
    {
        private readonly DemoService _service;

        public ItemsController()
        {
            this._service = new DemoService();
        }

        /// <summary>
        /// GET api/items/raw/{id}
        /// Throws exceptions from the service directly (demonstrates filter/global handler).
        /// </summary>
        [HttpGet]
        [Route("raw/{id:int}")]
        public IHttpActionResult GetRaw(int id)
        {
            // No try/catch: exception will flow to CustomExceptionFilter -> GlobalErrorHandler
            string item = this._service.GetItem(id);
            return Ok(item);
        }

        /// <summary>
        /// GET api/items/safe/{id}
        /// Demonstrates controller-level try/catch where we translate exceptions to responses.
        /// </summary>
        [HttpGet]
        [Route("safe/{id:int}")]
        public IHttpActionResult GetSafe(int id)
        {
            try
            {
                string item = this._service.SafeOperation(id);
                return Ok(item);
            }
            catch (System.ComponentModel.DataAnnotations.ValidationException vex)
            {
                ErrorResponse payload = new ErrorResponse
                {
                    Error = "Validation error",
                    Detail = vex.Message
                };
                return Content(HttpStatusCode.BadRequest, payload);
            }
            catch (BusinessException bex)
            {
                ErrorResponse payload = new ErrorResponse
                {
                    Error = bex.Message,
                    Detail = bex.Detail
                };
                return Content(bex.StatusCode, payload);
            }
        }

        /// <summary>
        /// GET api/items/localcatch/{id}
        /// Shows catching an unexpected exception and returning 500 with a friendly message.
        /// </summary>
        [HttpGet]
        [Route("localcatch/{id:int}")]
        public IHttpActionResult GetLocalCatch(int id)
        {
            try
            {
                string item = this._service.GetItem(id);
                return Ok(item);
            }
            catch (System.Exception ex)
            {
                // Log here as needed, then return friendly 500 to client
                ErrorResponse payload = new ErrorResponse
                {
                    Error = "Server error",
                    Detail = "We could not process your request: " + ex.Message
                };
                return Content(HttpStatusCode.InternalServerError, payload);
            }
        }
    }
}