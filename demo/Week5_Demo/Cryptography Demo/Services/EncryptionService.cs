using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_Demo.Services
{
    /// <summary>
    /// Handles encryption and decryption of sensitive data using AES algorithm.
    /// </summary>
    public class EncryptionService
    {
        private readonly byte[] _key;

        /// <summary>
        /// Initializes the EncryptionService with a key derived from a user-provided password.
        /// </summary>
        public EncryptionService(string password)
        {
            // Derive a 256-bit key from the password using SHA256
            _key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        /// Encrypts a plain text message using AES algorithm and returns Base64 string.
        /// </summary>
        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = RandomNumberGenerator.GetBytes(16); // 128-bit IV
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    // Write IV first, so we can use it during decryption
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts a Base64-encoded encrypted message using AES algorithm.
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            byte[] fullCipher = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extract IV from beginning of the cipher
                byte[] iv = new byte[16];
                Array.Copy(fullCipher, iv, iv.Length);
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(fullCipher, iv.Length, fullCipher.Length - iv.Length);
                    cryptoStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
