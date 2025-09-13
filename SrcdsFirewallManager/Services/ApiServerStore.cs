using SrcdsFirewallManager.Extenstions;
using SrcdsFirewallManager.Models.DTOs;
using SrcdsFirewallManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SrcdsFirewallManager.Services
{
    /// <summary>
    /// A service for communication with the API.
    /// </summary>
    internal sealed class ApiServerStore : IServerStore, IDisposable
    {

        /// <summary>
        /// Used for communication with the API.
        /// </summary>
        private readonly HttpClient _http = new()
        {
            BaseAddress = new Uri("https://api.steampowered.com/"),
            Timeout = TimeSpan.FromSeconds(30),
        };

        /// <inheritdoc/>
        public async Task<IDictionary<string, IEnumerable<Relay>>> GetServersAsync(CancellationToken cancellationToken = default)
        {
            return await _http.GetObjectAsync<Response>("ISteamApps/GetSDRConfig/v1/?appid=730", cancellationToken).ContinueWith(response =>
            {
                return response.Result?.Datacenters.Select(center => new
                {
                    Name = center.Value.Description,
                    Relays = center.Value.Relays ?? Enumerable.Empty<Relay>(),
                }).Where(center => center.Relays.Any()).ToDictionary(center => center.Name, center => center.Relays) ?? [];
            });
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _http.Dispose();
        }

    }
}
