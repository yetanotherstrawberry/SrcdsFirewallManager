using SrcdsFirewallManager.Services.Interfaces;
using System;

namespace SrcdsFirewallManager.Services
{
    /// <summary>
    /// A service for accessing the UI using <see cref="Action"/>s.
    /// </summary>
    /// <param name="messenger">Used for showing messages.</param>
    /// <param name="reporter">Used for reporting error messages.</param>
    /// <param name="invoker">Used for accessing the UI.</param>
    internal sealed class ActionsViewService(Action<string> messenger, Action<string>? reporter, Action<Action>? invoker) : IViewService
    {

        /// <summary>
        /// A field for <see cref="Execute(Action)"/>.
        /// </summary>
        private readonly Action<Action> _invoker = invoker ?? (action => action());

        /// <summary>
        /// A field for <see cref="ShowMessage(string)"/>.
        /// </summary>
        private readonly Action<string> _messenger = messenger;

        /// <summary>
        /// A field for <see cref="ShowError(string)"/>.
        /// </summary>
        private readonly Action<string> _reporter = reporter ?? messenger;

        /// <inheritdoc/>
        public void Execute(Action action) => _invoker(action);

        /// <inheritdoc/>
        public void ShowMessage(string message) => Execute(() => _messenger(message));

        /// <inheritdoc/>
        public void ShowError(string message) => Execute(() => _reporter(message));

    }
}
