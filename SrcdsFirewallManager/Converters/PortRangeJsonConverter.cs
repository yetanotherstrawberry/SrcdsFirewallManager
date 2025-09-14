using SrcdsFirewallManager.Models.DTOs;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SrcdsFirewallManager.Converters
{
    /// <summary>
    /// A <see cref="JsonConverter{T}"/> for the <see cref="PortRange"/>.
    /// </summary>
    internal sealed class PortRangeJsonConverter : JsonConverter<PortRange>
    {

        /// <inheritdoc/>
        public override PortRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var isArray = false;
            ushort? start = null, end = null;
            
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        if (isArray) throw new FormatException(nameof(JsonTokenType.StartArray));
                        isArray = true;
                        break;
                    case JsonTokenType.Number:
                        var number = reader.GetUInt16();
                        if (number < 0) throw new FormatException();
                        if (!start.HasValue) start = number;
                        else if (!end.HasValue) end = number;
                        else throw new FormatException();
                        break;
                    case JsonTokenType.EndArray:
                        if (!start.HasValue || !end.HasValue) throw new InvalidCastException();
                        if (start > end) (end, start) = (start, end);
                        return new PortRange()
                        {
                            Start = start.Value,
                            End = end.Value,
                        };
                    default:
                        throw new FormatException(nameof(reader.TokenType));
                }
            }
            throw new FormatException(nameof(reader.Read));
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, PortRange value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Start);
            writer.WriteNumberValue(value.End);
            writer.WriteEndArray();
        }

    }
}
