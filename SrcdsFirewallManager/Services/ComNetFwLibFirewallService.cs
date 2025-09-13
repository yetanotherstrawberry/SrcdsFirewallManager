using NetFwTypeLib;
using SrcdsFirewallManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace SrcdsFirewallManager.Services
{
    /// <summary>
    /// A server for controlling the firewall using <see cref="NetFwTypeLib"/>.
    /// </summary>
    [RequiresUnreferencedCode(nameof(NetFwTypeLib))]
    internal sealed class ComNetFwLibFirewallService : IFirewallService
    {

        /// <inheritdoc/>
        public void AddRule(string name, IEnumerable<IPAddress> addresses, Tuple<ushort, ushort> range, bool block = true, bool? udp = null, bool outbound = true)
        {
            var rule = CreateFirewallRule();
            rule.Enabled = true;
            rule.Name = string.Join("_", nameof(SrcdsFirewallManager), name);
            rule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
            rule.RemoteAddresses = string.Join(",", addresses);
            rule.Action = block ? NET_FW_ACTION_.NET_FW_ACTION_BLOCK : NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            rule.Protocol = (int)(udp.HasValue ? (udp.Value ? NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP : NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP) : NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY);
            rule.RemotePorts = udp.HasValue ? string.Join("-", range.Item1, range.Item2) : null;
            rule.Direction = outbound ? NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT : NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            Policy.Rules.Add(rule);
        }

        /// <inheritdoc/>
        public bool CheckStatus() => FirewallEnabled;

        /// <inheritdoc/>
        public void ResetAllRules()
        {
            foreach (var rule in GetRules())
            {
                Policy.Rules.Remove(rule.Name);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetBlockingRulesNames() => GetRules().Select(rule => rule.Name.Replace(nameof(SrcdsFirewallManager), string.Empty).TrimStart('_'));

        /// <summary>
        /// Gets rules that were created by using the program.
        /// </summary>
        /// <returns><see cref="INetFwRule"/>s with <see cref="INetFwRule.Name"/> beginnng with <see cref="SrcdsFirewallManager"/>.</returns>
        private IEnumerable<INetFwRule> GetRules()
        {
            return Policy.Rules.OfType<INetFwRule>().Where(rule => rule.Name.StartsWith(nameof(SrcdsFirewallManager)));
        }

        /// <summary>
        /// Indicathes whether the firewall is enabled in current <see cref="Profiles"/>.
        /// </summary>
        private bool FirewallEnabled => Policy.FirewallEnabled[Profiles];

        /// <summary>
        /// Current firewall policy.
        /// </summary>
        private INetFwPolicy2 Policy { get; }

        /// <summary>
        /// Currently active firewall profiles.
        /// </summary>
        private NET_FW_PROFILE_TYPE2_ Profiles => (NET_FW_PROFILE_TYPE2_)Policy.CurrentProfileTypes;

        /// <summary>
        /// Creates blank irewall rule.
        /// </summary>
        /// <returns>A <see langword="new"/> see <see cref="INetFwRule"/>.</returns>
        /// <exception cref="TypeInitializationException">Unable to <see cref="Activator.CreateInstance(Type)"/> on <see cref="FirewallRuleType"/>.</exception>
        private static INetFwRule CreateFirewallRule()
        {
            var instance = Activator.CreateInstance(FirewallRuleType) ?? throw new TypeInitializationException(typeof(INetFwRule).FullName, new ArgumentNullException());
            return instance as INetFwRule ?? throw new TypeInitializationException(typeof(INetFwRule).FullName, new InvalidCastException());
        }

        public ComNetFwLibFirewallService()
        {
            var instance = Activator.CreateInstance(FirewallPolicy) ?? throw new TypeInitializationException(typeof(INetFwPolicy2).FullName, new ArgumentNullException());
            Policy = instance as INetFwPolicy2 ?? throw new TypeInitializationException(typeof(INetFwPolicy2).FullName, new InvalidCastException());
        }

        /// <summary>
        /// <see cref="Type"/> of <see cref="COM_FIREWALL_POLICY"/>.
        /// </summary>
        private static Type FirewallPolicy => Type.GetTypeFromProgID(COM_FIREWALL_POLICY) ?? throw new TypeAccessException(COM_FIREWALL_POLICY);

        /// <summary>
        /// <see cref="Type"/> of <see cref="COM_FIREWALL_RULE"/>.
        /// </summary>
        private static Type FirewallRuleType => Type.GetTypeFromProgID(COM_FIREWALL_RULE) ?? throw new TypeAccessException(COM_FIREWALL_RULE);

        /// <summary>
        /// Names of COM modules.
        /// </summary>
        private const string COM_FIREWALL_RULE = "HNetCfg.FWRule", COM_FIREWALL_POLICY = "HNetCfg.FwPolicy2";

    }
}
