using System.Threading.Tasks;
using Autofac;
using Plutus.Core.DI.Modules;
using Plutus.Core.Interfaces;
using Xunit;

namespace Plutus.Tests.Integration.TATests
{
    public class TAHandlerTests : TestBase
    {
        public TAHandlerTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<BinanceModule>();
            builder.RegisterModule<TAModule>();

            base.SetContainer(builder);
        }

    }
}