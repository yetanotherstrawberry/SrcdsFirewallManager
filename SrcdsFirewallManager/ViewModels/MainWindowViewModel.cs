using SrcdsFirewallManager.Models.DTOs;
using SrcdsFirewallManager.Models.Internal;
using SrcdsFirewallManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SrcdsFirewallManager.ViewModels
{
    /// <summary>
    /// Main viewmodel.
    /// </summary>
    /// <param name="firewallService">A service for managing the firewall.</param>
    /// <param name="serverStore">A service for retrieving data.</param>
    internal sealed class MainWindowViewModel(IFirewallService firewallService, IServerStore serverStore, IViewService viewService) : ViewModelBase
    {

        /// <summary>
        /// Used to control the firewall.
        /// </summary>
        private readonly IFirewallService _firewallService = firewallService;

        /// <summary>
        /// Used to retrieve the data.
        /// </summary>
        private readonly IServerStore _serverStore = serverStore;

        /// <summary>
        /// Used for accessing the UI.
        /// </summary>
        private readonly IViewService _viewService = viewService;

        /// <summary>
        /// Field for <see cref="Servers"/>.
        /// </summary>
        private IList<Selectable<KeyValuePair<string, IEnumerable<Relay>>>> _servers = [];

        /// <summary>
        /// Servers.
        /// </summary>
        public IList<Selectable<KeyValuePair<string, IEnumerable<Relay>>>> Servers
        {
            get => _servers;
            set => SetProperty(ref _servers, value);
        }

        /// <summary>
        /// Currently selected <see cref="Servers"/>.
        /// </summary>
        private IEnumerable<KeyValuePair<string, IEnumerable<Relay>>> Selection => Servers.Where(selection => selection.Selected).Select(selection => selection.Value);

        /// <summary>
        /// Field for <see cref="DownloadCommand"/>.
        /// </summary>
        private ICommand? _downloadCommand;

        /// <summary>
        /// Executed to get current servers.
        /// </summary>
        public ICommand DownloadCommand
        {
            get
            {
                _downloadCommand ??= CreateCommand(async () =>
                {
                    await _serverStore.GetServersAsync().ContinueWith(task =>
                    {
                        var rules = _firewallService.GetBlockingRulesNames();
                        _viewService.Execute(() =>
                        {
                            Servers = [.. task.Result.Select(entity => new Selectable<KeyValuePair<string, IEnumerable<Relay>>>(entity))];
                            var servers = Servers.ToDictionary(server => server.Value.Key, server => server);
                            foreach (var rule in rules)
                            {
                                if (servers.TryGetValue(rule, out var server)) server.Selected = true;
                            }
                        });
                    });
                });
                return _downloadCommand;
            }
        }

        /// <summary>
        /// Field for <see cref="RefreshCommand"/>.
        /// </summary>
        private ICommand? _deleteCommand;

        /// <summary>
        /// Exetued in order to delete all rules.
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                _deleteCommand ??= CreateCommand(() =>
                {
                    _firewallService.ResetAllRules();
                    DownloadCommand.Execute(null);
                });
                return _deleteCommand;
            }
        }

        /// <summary>
        /// Field for <see cref="ApplyCommand"/>.
        /// </summary>
        private ICommand? _applyCommand;

        /// <summary>
        /// Executed after server selection has been changed.
        /// </summary>
        public ICommand ApplyCommand
        {
            get
            {
                _applyCommand ??= CreateCommand(() =>
                {
                    var selection = Selection.ToList();
                    _firewallService.ResetAllRules();
                    foreach (var server in selection)
                    {
                        _firewallService.AddRule(server.Key, server.Value.Select(relay => relay.Address), new Tuple<ushort, ushort>(server.Value.Min(relay => relay.Range.Start), server.Value.Max(relay => relay.Range.End)), udp: true);
                    }
                    DownloadCommand.Execute(null);
                });
                return _applyCommand;
            }
        }

    }
}
