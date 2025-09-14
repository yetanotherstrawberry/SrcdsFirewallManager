namespace SrcdsFirewallManager.Models.Internal
{
    /// <summary>
    /// Allows to mark selection.
    /// </summary>
    /// <typeparam name="TValue">Marked entity.</typeparam>
    /// <param name="instance">An instance that is selectable.</param>
    internal sealed class Selectable<TValue>(TValue instance) : ObservableModelBase
    {

        /// <summary>
        /// Field for <see cref="Selected"/>.
        /// </summary>
        private bool _selected;

        /// <summary>
        /// Indicates wtheret the current entity is selected (<see langword="true"/> or not. Notifies on change.
        /// </summary>
        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        /// <summary>
        /// An instance of <typeparamref name="TValue"/> that has been attached to <see langword="this"/> <see cref="Selectable{TValue}"/>.
        /// </summary>
        public TValue Value { get; } = instance;

    }
}
