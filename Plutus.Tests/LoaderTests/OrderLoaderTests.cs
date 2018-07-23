using Autofac;
using Plutus.Core.Helpers;
using Plutus.Core.Interfaces;
using Xunit;

namespace Plutus.Tests.LoaderTests
{
    public class OrderLoaderTests : LoaderTestBase
    {
        [Fact]
        public void CanLoadOrders()
        {
            var order1 = new OrderConfiguration()
            {
                Base = "BTC",
                Symbol = "XRP",
                LossStop = 11m,
                ProfitStop = 8m,
                BuyAmount = 1m
            };

            var order2 = new OrderConfiguration()
            {
                Base = "LTC",
                Symbol = "BTC",
                LossStop = 15m,
                ProfitStop = 12m,
                BuyAmount = 2.5m
            };

            var loader = Container.Resolve<IOrdersLoader>();

            Assert.True(loader.Orders.Count == 2);

            var firstOrder = loader.Orders[0];
            var secondOrder = loader.Orders[1];

            var firstOrdersEqual = order1.Base.Equals(firstOrder.Base) &&
                                   order1.Symbol.Equals(firstOrder.Symbol) &&
                                   order1.ProfitStop.Equals(firstOrder.ProfitStop) &&
                                   order1.LossStop.Equals(firstOrder.LossStop) &&
                                   order1.BuyAmount.Equals(firstOrder.BuyAmount);

            Assert.True(firstOrdersEqual);

            var secondOrdersEqual = order2.Base.Equals(secondOrder.Base) &&
                                    order2.Symbol.Equals(secondOrder.Symbol) &&
                                    order2.ProfitStop.Equals(secondOrder.ProfitStop) &&
                                    order2.LossStop.Equals(secondOrder.LossStop) &&
                                    order2.BuyAmount.Equals(secondOrder.BuyAmount);

            Assert.True(secondOrdersEqual);
        }
    }
}