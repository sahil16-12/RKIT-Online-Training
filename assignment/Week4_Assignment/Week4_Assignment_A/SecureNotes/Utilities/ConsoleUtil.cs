using System;
using System.Text;

/// <summary>
/// Console helper utilities (password masking etc.).
/// </summary>
namespace SecureNotes.Utilities
{
    public static class ConsoleUtil
    {
        /// <summary>
        /// Read a password from console with masking. The returned string is allocated normally.
        /// Caller should minimize lifetime and overwrite when possible.
        /// </summary>
        public static string ReadPasswordMasked()
        {
            StringBuilder builder = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (builder.Length > 0)
                    {
                        builder.Remove(builder.Length - 1, 1);
                        Console.Write("\b \b");
                    }

                    continue;
                }

                // Only accept visible characters
                if (!char.IsControl(keyInfo.KeyChar))
                {
                    builder.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            }

            string password = builder.ToString();

            // Try to zero the string builder contents (best-effort)
            for (int i = 0; i < builder.Length; i++)
            {
                builder[i] = '\0';
            }

            return password;
        }
    }
}
