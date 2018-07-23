using Autofac;
using Plutus.Core.Helpers;
using Plutus.Core.Interfaces;

namespace Plutus.Core.DI.Modules
{
    public class TAModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<TAHandler>().As<ITAHandler>().SingleInstance();
        }
    }
}