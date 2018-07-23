using Autofac;
using Plutus.Core.DI.Modules;
using Plutus.Core.Interfaces;
using Xunit;

namespace Plutus.Tests.ExchangeTests
{
    public class BinanceHandlerTests : TestBase
    {
        public BinanceHandlerTests() 
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<BinanceModule>();

            SetContainer(builder);
        }

        [Fact]
        public async void CanPing()
        {
            var handler = Container.Resolve<IBinanceHandler>();

            var online = await handler.Ping();

            Assert.True(online);
        }

        [Fact]
        public async void CanGetBalance()
        {
            var handler = Container.Resolve<IBinanceHandler>();

            var trxBalance = await handler.GetBalance("TRX");
            
            Assert.NotNull(trxBalance);
            Assert.Equal(0.66400000m, trxBalance.Value);
        }
        
        [Fact]
        public async void CanGetPrice()
        {
            var handler = Container.Resolve<IBinanceHandler>();

            var price = await handler.GetPrice("BTC", "XRP");
            
            Assert.True(price > 0);
        }

        [Fact]
        public async void CanGetCandlesticks()
        {
            var handler = Container.Resolve<IBinanceHandler>();

            var candlesticks = await handler.GetCandlesticks("BTC", "XRP");
            
            Assert.NotNull(candlesticks);
            Assert.NotEmpty(candlesticks);
        }

        [Fact]
        public async void CanBuy()
        {
            var handler = Container.Resolve<IBinanceHandler>();

            await handler.BuyTest("BTC", "XRP", 1m);
        }
        
        [Fact]
        public async void CanSell()
        {
            var handler = Container.Resolve<IBinanceHandler>();

            await handler.SellTest("BTC", "XRP", 1m);
        }
    }
}