using System;

/// <summary>
/// Encrypted payload representation stored inside note JSON on disk.
/// Contains ciphertext, nonce (IV) and authentication tag (for AES-GCM).
/// </summary>
namespace SecureNotes.Models
{
    public sealed class EncryptedPayload
    {
        /// <summary>Base64-encoded ciphertext bytes.</summary>
        public string CipherTextBase64 { get; set; }

        /// <summary>Base64-encoded nonce/IV.</summary>
        public string NonceBase64 { get; set; }

        /// <summary>Base64-encoded authentication tag.</summary>
        public string TagBase64 { get; set; }

        /// <summary>Constructor.</summary>
        public EncryptedPayload()
        {
            CipherTextBase64 = string.Empty;
            NonceBase64 = string.Empty;
            TagBase64 = string.Empty;
        }
    }
}
