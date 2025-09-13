using SrcdsFirewallManager.Models.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SrcdsFirewallManager.Services.Interfaces
{
    /// <summary>
    /// A service for managing the store of servers.
    /// </summary>
    internal interface IServerStore
    {

        /// <summary>
        /// Gets the servers.
        /// </summary>
        /// <param name="cancellationToken">Token that will cancel the HTTP request.</param>
        /// <returns>A <see langword="new"/> <see cref="IDictionary{TKey, TValue}"/> with <see cref="IDictionary{TKey, TValue}.Keys"/> as names of the servers and <see cref="IDictionary{TKey, TValue}.Values"/> as <see cref="Relay"/>s.</returns>
        public Task<IDictionary<string, IEnumerable<Relay>>> GetServersAsync(CancellationToken cancellationToken = default);

    }
}
