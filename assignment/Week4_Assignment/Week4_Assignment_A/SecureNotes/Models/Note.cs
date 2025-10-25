using System;
using System.Text.Json.Serialization;
using SecureNotes.Models;

/// <summary>
/// Represents a note stored in the vault. The Body property is plaintext when loaded into memory.
/// When persisted to disk, Body is stored as EncryptedPayload in file.
/// </summary>
namespace SecureNotes.Models
{
    public sealed class Note
    {
        /// <summary>Unique identifier for the note.</summary>
        public Guid Id { get; set; }

        /// <summary>Human-facing title of the note.</summary>
        public string Title { get; set; }

        /// <summary>Plaintext body of the note in memory. Persisted encrypted to disk.</summary>
        public string Body { get; set; }

        /// <summary>Creation timestamp.</summary>
        [JsonConverter(typeof(SecureNotes.Json.DateTimeOffsetConverter))]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>Last updated timestamp.</summary>
        [JsonConverter(typeof(SecureNotes.Json.DateTimeOffsetConverter))]
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>Constructor.</summary>
        public Note()
        {
            Id = Guid.Empty;
            Title = string.Empty;
            Body = string.Empty;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
