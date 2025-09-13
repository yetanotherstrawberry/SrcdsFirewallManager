using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SrcdsFirewallManager.Converters
{
    internal sealed class IpAddressJsonConverter : JsonConverter<IPAddress>
    {

        public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var read = reader.GetString();
            return read is null ? null : IPAddress.Parse(read);
        }

        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

    }
}
