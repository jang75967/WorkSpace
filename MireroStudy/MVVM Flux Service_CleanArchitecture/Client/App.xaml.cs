using Client.Common.DI;
using Client.Extensions;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceProvider = ConfigureServices();

            DISource.Resolver = Resolve;

            var mainView = ServiceProvider.GetRequiredService<MainWindow>();
            mainView.Show();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.Configure();
            services.AddFluxor(o => o.ScanAssemblies(
                typeof(App).Assembly,
                typeof(Domain.Count.CountState).Assembly
            ));

            ServiceConfigurator.Configure(services);

            return services.BuildServiceProvider();
        }

        object Resolve(Type type, object key, string name) => type == null ? null : ServiceProvider.GetService(type);
    }
}
