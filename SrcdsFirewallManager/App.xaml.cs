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
    /// Interaction logic for the <see cref="Application"/>.
    /// </summary>
    internal sealed partial class App : Application
    {

        /// <summary>
        /// Used for service registration.
        /// </summary>
        private ServiceProvider? ServiceProvider { get; set; } = null;

        /// <summary>
        /// Gets the <see cref="Application.Current"/> instance.
        /// </summary>
        private static App Self => (App)Current;

        /// <summary>
        /// Gets an instance of a viewmodel attached to the view.
        /// </summary>
        /// <param name="viewType"><see cref="Type"/> of the view.</param>
        /// <returns>An instance of the viewmodel.</returns>
        /// <exception cref="TypeAccessException">Could not create an instance.</exception>
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
        /// Registers views and viewmodels.
        /// </summary>
        /// <param name="serviceCollection">An instance to be used for registration.</param>
        private void RegisterViews(IServiceCollection serviceCollection)
        {
            RegisterView<MainWindow, MainWindowViewModel>(serviceCollection, singleton: true);
        }

        /// <summary>
        /// Registers services and views.
        /// </summary>
        /// <param name="serviceCollection">An instance to be used for registration.</param>
        private void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IServerStore, ApiServerStore>();
            serviceCollection.AddScoped<IFirewallService, ComNetFwLibFirewallService>();

            RegisterViews(serviceCollection);
        }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs startupArgs)
        {
            DispatcherUnhandledException += ExceptionMessageHandler;
            base.OnStartup(startupArgs);

            var serviceCollection = new ServiceCollection();
            RegisterServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            (MainWindow = ServiceProvider.GetRequiredService<MainWindow>()).Show();
        }

        /// <summary>
        /// Handles unhandled <see cref="Exception"/>s by showing a message.
        /// </summary>
        /// <param name="sender">An <see cref="object"/> that sent the <see cref="Exception"/>.</param>
        /// <param name="exceptionArgs"><see cref="EventArgs"/> of the <see cref="Exception"/>. <see cref="DispatcherUnhandledExceptionEventArgs.Exception"/> will be used to retrieve the <see cref="Exception.Message"/>.</param>
        private void ExceptionMessageHandler(object sender, DispatcherUnhandledExceptionEventArgs exceptionArgs)
        {
            exceptionArgs.Handled = true;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                MessageBox.Show(MainWindow, exceptionArgs.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            });
            if (MainWindow is null) Shutdown();
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs exitArgs)
        {
            base.OnExit(exitArgs);
            ServiceProvider?.Dispose();
            DispatcherUnhandledException -= ExceptionMessageHandler;
        }

    }
}
