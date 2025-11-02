using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_Demo.Services
{
    /// <summary>
    /// Responsible for creating and verifying password hashes securely using PBKDF2.
    /// </summary>
    public class PasswordService
    {
        /// <summary>
        /// Generates a salted hash for the given password.
        /// </summary>
        public (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16); // 128-bit random salt

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 256-bit hash

            return (Convert.ToBase64String(hash), Convert.ToBase64String(saltBytes));
        }

        /// <summary>
        /// Verifies that a given password matches its previously stored hash and salt.
        /// </summary>
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
            byte[] computedHash = pbkdf2.GetBytes(32);

            return Convert.ToBase64String(computedHash).Equals(storedHash, StringComparison.Ordinal);
        }
    }
}
