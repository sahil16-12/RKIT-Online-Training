using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Cryptographic helper: PBKDF2 key derivation and AES-GCM encryption/decryption.
/// Zeroes sensitive buffers where possible.
/// </summary>
namespace SecureNotes.Services
{
    public sealed class CryptoService : IDisposable
    {
        /// <summary>Salt bytes used for PBKDF2.</summary>
        private readonly byte[] _salt;

        /// <summary>Iteration count for PBKDF2.</summary>
        private readonly int _iterations;

        /// <summary>Derived key for AES-256.</summary>
        private byte[] _key;

        /// <summary>Whether the instance has been disposed.</summary>
        private bool _disposed;

        /// <summary>
        /// Construct CryptoService by deriving a key from passphrase + salt.
        /// </summary>
        public CryptoService(string passphrase, byte[] salt, int iterations)
        {
            if (salt == null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            _salt = new byte[salt.Length];
            Array.Copy(salt, _salt, salt.Length);
            _iterations = iterations;

            _key = DeriveKey(passphrase, _salt, _iterations);
        }

        /// <summary>
        /// Derive 256-bit key using PBKDF2 (Rfc2898).
        /// </summary>
        private static byte[] DeriveKey(string passphrase, byte[] salt, int iterations)
        {
            using Rfc2898DeriveBytes kdf = new Rfc2898DeriveBytes(passphrase, salt, iterations, HashAlgorithmName.SHA256);
            byte[] derived = kdf.GetBytes(32); // 256 bits
            return derived;
        }

        /// <summary>
        /// Encrypt plaintext bytes with AES-GCM. Returns tuple (ciphertext, nonce, tag).
        /// </summary>
        public (byte[] CipherText, byte[] Nonce, byte[] Tag) Encrypt(byte[] plain)
        {
            byte[] nonce = new byte[12];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(nonce);

            byte[] cipher = new byte[plain.Length];
            byte[] tag = new byte[16];

            using AesGcm aes = new AesGcm(_key);
            aes.Encrypt(nonce, plain, cipher, tag, null);

            return (cipher, nonce, tag);
        }

        /// <summary>
        /// Decrypt with AES-GCM using stored key.
        /// </summary>
        public byte[] Decrypt(byte[] cipher, byte[] nonce, byte[] tag)
        {
            byte[] plaintext = new byte[cipher.Length];
            using AesGcm aes = new AesGcm(_key);
            aes.Decrypt(nonce, cipher, tag, plaintext, null);
            return plaintext;
        }

        /// <summary>
        /// Securely zero key and salt and dispose.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (_key != null)
            {
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(_key);
                _key = Array.Empty<byte>();
            }

            if (_salt != null)
            {
                System.Security.Cryptography.CryptographicOperations.ZeroMemory(_salt);
            }
        }
    }
}
