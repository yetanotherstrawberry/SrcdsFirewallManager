namespace SrcdsFirewallManager.Models.Internal
{
    internal sealed class Selectable<TValue> : ObservableModelBase
    {

        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public required TValue Value { get; set; }

    }
}
