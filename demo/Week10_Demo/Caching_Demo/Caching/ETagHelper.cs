using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Caching_Demo.Caching
{
    /// <summary>
    /// Helper to compute ETag strings from objects by hashing their JSON representation.
    /// </summary>
    public static class ETagHelper
    {
        /// <summary>
        /// Compute a quoted ETag (e.g. "abc123").
        /// </summary>
        /// <param name="obj">Object to compute ETag for.</param>
        /// <returns>ETag string including quotes.</returns>
        public static string ComputeETag(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                byte[] hash = sha256.ComputeHash(bytes);
                string hex = BytesToHex(hash);
                return "\"" + hex + "\""; // quoted per RFC
            }
        }

        private static string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}