using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Plutus.Core.DI.Modules;

namespace Plutus.Core.DI
{
    public static class AutofacBootstrapper
    {
        public static IContainer Init()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddAutofac();
            serviceCollection.AddLogging();

            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);
            
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<DatabaseModule>();
            builder.RegisterModule<ExchangeModule>();
            builder.RegisterModule<TAModule>();
            builder.RegisterModule<PlutusModule>();

            return builder.Build();
        }
    }
}