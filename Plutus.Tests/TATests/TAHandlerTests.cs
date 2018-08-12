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

        [Fact]
        public async Task CanCalculateRsi()
        {
            var taHandler = Container.Resolve<ITAHandler>();
            var binanceHandler = Container.Resolve<IBinanceHandler>();

            var candles = await binanceHandler.GetCandlesticks("BTC", "XRP");

            var rsi = taHandler.CalculateRSI(candles);
            
            Assert.NotNull(rsi);
            Assert.InRange(rsi.Value, 0, 100);
        }
    }
}