using Swagger_Demo.Models;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Swagger_Demo.Controllers
{
    /// <summary>
    /// Controller for handling image uploads via file or base64 encoding.
    /// </summary>
    [RoutePrefix("api/image")]
    public class ImageController : ApiController
    {
        /// <summary>
        /// POST api/image/upload
        /// Upload an image file (supports multipart/form-data).
        /// </summary>
        /// <remarks>
        /// This endpoint accepts an image file upload via multipart/form-data.
        /// Supported formats: JPG, PNG, GIF, BMP.
        /// Maximum file size: 10MB.
        /// </remarks>
        /// <returns>Success message with image details.</returns>
        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> UploadImage()
        {
            // Check if the request contains multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported media type. Please use multipart/form-data.");
            }

            try
            {
                // Read the multipart content
                MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                // Check if any file was uploaded
                if (provider.Contents.Count == 0)
                {
                    return BadRequest("No file was uploaded.");
                }

                // Get the first file from the request
                HttpContent fileContent = provider.Contents[0];
                string fileName = fileContent.Headers.ContentDisposition.FileName?.Trim('"');
                string contentType = fileContent.Headers.ContentType?.MediaType;

                // Validate content type
                if (!IsValidImageContentType(contentType))
                {
                    return BadRequest("Invalid file type. Only image files (JPG, PNG, GIF, BMP) are allowed.");
                }

                // Read the file stream
                var fileStream = await fileContent.ReadAsStreamAsync();

                // Validate file size (10MB max)
                if (fileStream.Length > 10 * 1024 * 1024)
                {
                    return BadRequest("File size exceeds the maximum limit of 10MB.");
                }

                // Validate that it's actually an image by trying to load it
                try
                {
                    using (Image image = Image.FromStream(fileStream))
                    {
                        var imageInfo = new
                        {
                            FileName = fileName,
                            ContentType = contentType,
                            Width = image.Width,
                            Height = image.Height,
                            Format = image.RawFormat.ToString(),
                            SizeInBytes = fileStream.Length,
                            Message = "Image uploaded successfully!"
                        };

                        // Here we would typically save the image to disk or database
                        // For demo purposes, we just return the image information

                        return Ok(imageInfo);
                    }
                }
                catch (ArgumentException)
                {
                    return BadRequest("The uploaded file is not a valid image.");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error processing image: {ex.Message}"));
            }
        }

        /// <summary>
        /// POST api/image/upload-base64
        /// Upload an image as a base64 encoded string.
        /// </summary>
        /// <remarks>
        /// This endpoint accepts an image as a base64 encoded string in the request body.
        /// The request should be a JSON object with the following structure:
        /// {
        ///   "base64Image": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA...",
        ///   "fileName": "myimage.png" (optional)
        /// }
        /// or just the base64 string without the data URI prefix.
        /// </remarks>
        /// <param name="request">Base64 image upload request.</param>
        /// <returns>Success message with image details.</returns>
        [HttpPost]
        [Route("upload-base64")]
        public IHttpActionResult UploadBase64Image([FromBody] Base64ImageRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Base64Image))
            {
                return BadRequest("Base64Image field is required.");
            }

            try
            {
                // Remove data URI prefix if present (e.g., "data:image/png;base64,")
                string base64String = request.Base64Image;
                if (base64String.Contains(","))
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                }

                // Convert base64 to byte array
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Validate file size (10MB max)
                if (imageBytes.Length > 10 * 1024 * 1024)
                {
                    return BadRequest("Image size exceeds the maximum limit of 10MB.");
                }

                // Validate that it's actually an image
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    try
                    {
                        using (var image = Image.FromStream(ms))
                        {
                            var imageInfo = new
                            {
                                FileName = string.IsNullOrWhiteSpace(request.FileName) ? "uploaded_image" : request.FileName,
                                Width = image.Width,
                                Height = image.Height,
                                Format = image.RawFormat.ToString(),
                                SizeInBytes = imageBytes.Length,
                                Message = "Base64 image uploaded successfully!"
                            };

                            // Here we would typically save the image to disk or database
                            // For demo purposes, we just return the image information

                            return Ok(imageInfo);
                        }
                    }
                    catch (ArgumentException)
                    {
                        return BadRequest("The provided base64 string does not represent a valid image.");
                    }
                }
            }
            catch (FormatException)
            {
                return BadRequest("Invalid base64 string format.");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error processing base64 image: {ex.Message}"));
            }
        }

        /// <summary>
        /// Validates if the content type is a valid image type.
        /// </summary>
        private bool IsValidImageContentType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
                return false;

            var validTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp" };
            return Array.Exists(validTypes, type => type.Equals(contentType, StringComparison.OrdinalIgnoreCase));
        }
    }


}