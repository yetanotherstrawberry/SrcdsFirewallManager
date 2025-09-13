namespace SrcdsFirewallManager.Models.Internal
{
    /// <summary>
    /// Allows to mark selection.
    /// </summary>
    /// <typeparam name="TValue">Marked entity.</typeparam>
    internal sealed class Selectable<TValue> : ObservableModelBase
    {

        /// <summary>
        /// Field for <see cref="Selected"/>.
        /// </summary>
        private bool _selected;

        /// <summary>
        /// Indicates wtheret the current entity is selected (<see langword="true"/> or not.
        /// </summary>
        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        /// <summary>
        /// An instance of <typeparamref name="TValue"/> that has been attached to <see langword="this"/> <see cref="Selectable{TValue}"/>.
        /// </summary>
        public required TValue Value { get; set; }

    }
}
