using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SecureNotes.Json
{
    /// <summary>
    /// Custom converter to ensure stable DateTimeOffset serialization in ISO-8601 (round-trip).
    /// </summary>
    public sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        /// <summary>
        /// Read DateTimeOffset from JSON string.
        /// </summary>
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (value == null)
            {
                return DateTimeOffset.MinValue;
            }

            DateTimeOffset parsed;
            bool success = DateTimeOffset.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out parsed);
            if (success)
            {
                return parsed;
            }

            return DateTimeOffset.MinValue;
        }

        /// <summary>
        /// Write DateTimeOffset as ISO 8601 string.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}
