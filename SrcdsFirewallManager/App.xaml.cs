using Microsoft.Extensions.DependencyInjection;
using SrcdsFirewallManager.Services;
using SrcdsFirewallManager.Services.Interfaces;
using SrcdsFirewallManager.ViewModels;
using SrcdsFirewallManager.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace SrcdsFirewallManager
{
    /// <summary>
    /// Interaction logic for "App.xaml".
    /// </summary>
    internal sealed partial class App : Application
    {

        /// <summary>
        /// Used for service registration.
        /// </summary>
        private ServiceProvider? ServiceProvider { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        private static App Self => (App)Current;

        public static object? GetViewModel(Type viewType)
        {
            var viewModel = Self.ViewModelLocator.TryGetValue(viewType, out var instance) ? instance : null;
            if (viewModel is null) return null;
            return Self.ServiceProvider?.GetService(viewModel) ?? Activator.CreateInstance(viewModel) ?? throw new TypeAccessException(viewModel.FullName);
        }

        /// <summary>
        /// Maps view to viewmodel.
        /// </summary>
        private Dictionary<Type, Type> ViewModelLocator { get; } = [];

        /// <summary>
        /// Registers the view and the viewmodel as singleton or transient services, as well as attaches one to another.
        /// </summary>
        /// <typeparam name="TView">The view to register.</typeparam>
        /// <typeparam name="TViewModel">The <see cref="FrameworkElement.DataContext"/> to register and attach to the <see cref="GetViewModel(Type)"/>.</typeparam>
        /// <param name="serviceCollection">Instance to register the services to.</param>
        /// <param name="singleton">Indicates whether the view should be registered as singleton (<see langword="true"/>) or as transient service.</param>
        private void RegisterView<TView, TViewModel>(IServiceCollection serviceCollection, bool singleton = false) where TView : class where TViewModel : class
        {
            if (singleton)
            {
                serviceCollection.AddSingleton<TView>();
                serviceCollection.AddSingleton<TViewModel>();
            }
            else
            {
                serviceCollection.AddTransient<TView>();
                serviceCollection.AddTransient<TViewModel>();
            }

            ViewModelLocator.Add(typeof(TView), typeof(TViewModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        private void RegisterViews(IServiceCollection serviceCollection)
        {
            RegisterView<MainWindow, MainWindowViewModel>(serviceCollection, singleton: true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        private void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IServerStore, ApiServerStore>();
            serviceCollection.AddScoped<IFirewallService, ComNetFwLibFirewallService>();

            RegisterViews(serviceCollection);
        }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs startupArgs)
        {
            DispatcherUnhandledException += ExceptionHandler;
            base.OnStartup(startupArgs);

            var serviceCollection = new ServiceCollection();
            RegisterServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            (MainWindow = ServiceProvider.GetRequiredService<MainWindow>()).Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="exceptionArgs"></param>
        private void ExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs exceptionArgs)
        {
            exceptionArgs.Handled = true;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                MessageBox.Show(MainWindow, exceptionArgs.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            });
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs exitArgs)
        {
            base.OnExit(exitArgs);
            ServiceProvider?.Dispose();
            DispatcherUnhandledException -= ExceptionHandler;
        }

    }
}
