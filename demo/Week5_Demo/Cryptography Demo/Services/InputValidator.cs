using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cryptography_Demo.Services
{
    /// <summary>
    /// Provides validation for user inputs to prevent code injection and unsafe data.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Validates that the given input text does not contain any malicious characters or scripts.
        /// </summary>
        public static bool IsValidText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Reject HTML or script tags to prevent XSS-type behavior
            Regex regex = new Regex(@"<script|</script|<|>", RegexOptions.IgnoreCase);
            return !regex.IsMatch(input);
        }
    }
}
