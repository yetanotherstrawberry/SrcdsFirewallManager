using System;
using System.Net;
using System.Text.Json.Serialization;

namespace SrcdsFirewallManager.Models.DTOs
{
    internal sealed class Relay
    {

        [JsonPropertyName("ipv4")]
        public required IPAddress Address { get; set; }

        [JsonPropertyName("port_range")]
        public required PortRange Range { get; set; }

    }
}
