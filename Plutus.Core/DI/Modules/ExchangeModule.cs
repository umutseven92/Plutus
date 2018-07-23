using Autofac;

namespace Plutus.Core.DI.Modules
{
    /// <inheritdoc />
    /// <summary>
    /// Register all exchange clients here.
    /// </summary>
    public class ExchangeModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<BinanceModule>();
        }
    }
}