using System;

namespace SrcdsFirewallManager.Services.Interfaces
{
    /// <summary>
    /// A service for accessing the UI.
    /// </summary>
    internal interface IViewService
    {

        /// <summary>
        /// Executes an <see cref="Action"/> so that it can access the UI.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to <see cref="Action.Invoke"/>.</param>
        public void Execute(Action action);

        /// <summary>
        /// Shows a message to the user.
        /// </summary>
        /// <param name="message">Text of the message.</param>
        public void ShowMessage(string message);

        /// <summary>
        /// Shows an error to the user.
        /// </summary>
        /// <param name="message">Text of the error.</param>
        public void ShowError(string message);

    }
}
