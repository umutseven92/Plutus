using Autofac;
using Microsoft.Extensions.Logging;
using Plutus.Core.Helpers;
using Plutus.Core.Interfaces;

namespace Plutus.Core.DI.Modules
{
    public class PlutusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c =>
            {
                var factory = c.Resolve<ILoggerFactory>();
                factory.AddConsole();
                return factory.CreateLogger<PlutusService>();
            }).As<ILogger>();

            builder.RegisterType<PlutusService>().As<IPlutusService>();
        }
    }
}