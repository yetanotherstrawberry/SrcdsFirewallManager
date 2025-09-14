using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SrcdsFirewallManager.Views
{
    /// <summary>
    /// Base for all <see cref="Window"/>s.
    /// </summary>
    internal abstract class WindowBase : Window
    {

        public WindowBase()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var viewModel = App.GetViewModel(GetType());
                if (viewModel != null) DataContext = viewModel;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
            }
        }

    }
}
