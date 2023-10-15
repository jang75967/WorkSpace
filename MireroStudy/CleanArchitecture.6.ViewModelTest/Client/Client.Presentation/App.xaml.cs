using Client.Business.Extensions;
using Client.Presentation.Extentions;
using Client.Presentation.Services;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Threading;

namespace Client.Presentation
{
    public partial class App
    {
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

                services.AddHostedService<ApplicationHostService>();

                services.AddWpfUI();
                services.AddViews();

                services.AddViewModels();
                services.AddAutoMapper();
                services.AddGrpcClientChannel(configuration);
                services.AddMediatR();
                services.AddSingleton<IMessenger, WeakReferenceMessenger>();


            }).Build();

        public static T GetService<T>() where T : class => (_host.Services.GetService(typeof(T)) as T)!;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            
        }
    }
}
