using System;
using System.Collections.Generic;
using System.Net;

namespace SrcdsFirewallManager.Services.Interfaces
{
    /// <summary>
    /// A service for managing the built-in firewall.
    /// </summary>
    internal interface IFirewallService
    {

        /// <summary>
        /// Creates a firewall rule and adds it to the current policy.
        /// </summary>
        /// <param name="name">Name of the rule.</param>
        /// <param name="addresses"><see cref="IPAddress"/>es to be included in the rule.</param>
        /// <param name="range">Ports that will be affected. Will be ignored is <paramref name="udp"/> <see langword="is"/> <see langword="null"/>.</param>
        /// <param name="block">Indicates whether the rule should block (<see langword="true"/>) the connection or allow it.</param>
        /// <param name="udp">Set to <see langword="true"/> to make the rule affect only the UDP protocol, <see langword="false"/> to only affect the TCP protocol or <see langword="null"/> to affect all protocols.</param>
        /// <param name="outbound">Set it to <see langword="true"/> to make the rule affect only outgoing connections or <see langword="false"/> to affect incoming connections.</param>
        public void AddRule(string name, IEnumerable<IPAddress> addresses, Tuple<ushort, ushort> range, bool block = true, bool? udp = null, bool outbound = true);

        /// <summary>
        /// Returns the status of built-in firewall.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if the firewall is enabled; otherwise <see langword="false"/>.</returns>
        public bool CheckStatus();

        /// <summary>
        /// Gets the names of the rules that were added.
        /// </summary>
        /// <returns>Names of rules.</returns>
        public IEnumerable<string> GetBlockingRulesNames();

        /// <summary>
        /// Removes all rules that have been created using the current program. The rules are identified by their name beginning with the root namespace.
        /// </summary>
        public void ResetAllRules();

    }
}
