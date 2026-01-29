using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;

namespace Exceptions_Demo
{
    /// <summary>
    /// A small service illustrating business logic that can throw different exceptions.
    /// Keep business rules and exception translation inside services.
    /// </summary>
    public class DemoService
    {
        /// <summary>
        /// Simulate retrieving an item. Throws different exceptions for demo:
        /// - id == 0 -> validation exception
        /// - id == -1 -> business not found
        /// - id < -1 -> unexpected error
        /// </summary>
        public string GetItem(int id)
        {
            if (id == 0)
            {
                throw new ValidationException("Id must be greater than zero.");
            }

            if (id == -1)
            {
                throw new BusinessException(
                    message: "Item not found",
                    detail: $"No item exists with id {id}.",
                    statusCode: HttpStatusCode.NotFound);
            }

            if (id < -1)
            {
                throw new InvalidOperationException("Data store read failure (simulated).");
            }

            // Normal successful result
            return "Item #" + id.ToString();
        }

        /// <summary>
        /// Simulate an operation that internally catches exceptions and wraps them into business exceptions
        /// when appropriate.
        /// </summary>
        public string SafeOperation(int id)
        {
            try
            {
                string result = GetItem(id);
                return result;
            }
            catch (ValidationException)
            {
                // rethrow validation exceptions for filters to handle
                throw;
            }
            catch (BusinessException)
            {
                // propagate business exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                // Wrap any unexpected error with a BusinessException to provide a nicer client message
                throw new BusinessException(
                    message: "Operation failed",
                    detail: "Internal error while fetching item: " + ex.Message,
                    statusCode: HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}