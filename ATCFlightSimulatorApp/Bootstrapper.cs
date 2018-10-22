
using System.Windows;
using Prism.Modularity;
using Prism.Autofac;
using Autofac;

using XPlane10DataAdapter;

using ATCFlightSimulatorApp.Views;
using ATCFlightSimulatorApp.Services;

namespace ATCFlightSimulatorApp
{
    public class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        { 
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Config()).As<IConfig>();
            builder.RegisterInstance(new ATCFlightClient()).As<IATCFlightClient>();
            builder.RegisterInstance(new XPlaneConnect()).As<IXPlaneConnect>();
            base.ConfigureContainerBuilder(builder);
        }

    }
}
