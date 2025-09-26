using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SrcdsFirewallManager.Models.DTOs
{
    internal class Response
    {

        [JsonPropertyName("pops")]
        public required IDictionary<string, Datacenter> Datacenters { get; set; }

        public required bool Success { get; set; }

    }
}
