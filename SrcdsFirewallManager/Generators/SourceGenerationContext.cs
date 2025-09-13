using SrcdsFirewallManager.Converters;
using SrcdsFirewallManager.Models.DTOs;
using System.Text.Json.Serialization;

namespace SrcdsFirewallManager.Generators
{

    [JsonSourceGenerationOptions(
        Converters = new[] { typeof(PortRangeJsonConverter), typeof(IpAddressJsonConverter) },
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(Response))]
    internal sealed partial class SourceGenerationContext : JsonSerializerContext { }

}
