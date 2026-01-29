using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swagger_Demo.Models
{
    /// <summary>
    /// Request model for base64 image upload.
    /// </summary>
    public class Base64ImageRequest
    {
        /// <summary>
        /// Base64 encoded image string (with or without data URI prefix).
        /// </summary>
        /// <example>data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA...</example>
        public string Base64Image { get; set; }

        /// <summary>
        /// Optional file name for the uploaded image.
        /// </summary>
        /// <example>myimage.png</example>
        public string FileName { get; set; }
    }

}