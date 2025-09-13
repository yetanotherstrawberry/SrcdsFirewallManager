using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SrcdsFirewallManager.Models.DTOs
{
    internal sealed class Datacenter
    {

        [JsonPropertyName("desc")]
        public required string Description { get; set; }

        public ICollection<Relay>? Relays { get; set; }

    }
}
