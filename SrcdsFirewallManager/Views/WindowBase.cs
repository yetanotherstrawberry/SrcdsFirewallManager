using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SrcdsFirewallManager.Views
{
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
