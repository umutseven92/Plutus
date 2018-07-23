using Autofac;
using Plutus.Core.Helpers;
using Plutus.Core.Interfaces;

namespace Plutus.Core.DI.Modules
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterType<SettingsLoader>().As<ISettingsLoader>()
                .WithParameter("path", "appsettings.json").SingleInstance();

            builder.RegisterType<OrdersLoader>().As<IOrdersLoader>()
                .WithParameter("path", "orders.json").SingleInstance();
        }
    }
}