using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SrcdsFirewallManager.ViewModels
{
    /// <summary>
    /// Handles <see langword="event"/>s for changing properties.
    /// </summary>
    internal abstract class ViewModelBase : ObservableObject
    {

        /// <summary>
        /// Field for <see cref="IsFree"/>.
        /// </summary>
        private bool _isFree = true;

        /// <summary>
        /// Indicates whether the UI should be disabled (<see langword="false"/>) as there is an ongoing operation.
        /// </summary>
        public bool IsFree
        {
            get => _isFree;
            set => SetProperty(ref _isFree, value);
        }

        /// <summary>
        /// Creates an <see cref="ICommand"/> for given <paramref name="action"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to execute.</param>
        /// <returns>A <see langword="new"/> <see cref="ICommand"/>.</returns>
        protected ICommand CreateCommand(Action action)
        {
            return new RelayCommand(() =>
            {
                try
                {
                    IsFree = false;
                    action();
                }
                finally
                {
                    IsFree = true;
                }
            });
        }

        /// <summary>
        /// Creates an <see cref="ICommand"/> for given <paramref name="function"/>.
        /// </summary>
        /// <param name="function">The <see cref="Task"/> to execute.</param>
        /// <returns>A <see langword="new"/> <see cref="ICommand"/>.</returns>
        protected ICommand CreateCommand(Func<Task> function)
        {
            return new AsyncRelayCommand(async () =>
            {
                try
                {
                    IsFree = false;
                    await function();
                }
                finally
                {
                    IsFree = true;
                }
            });
        }

    }
}
