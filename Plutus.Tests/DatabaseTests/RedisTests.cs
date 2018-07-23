using System.Linq;
using Autofac;
using Plutus.Core.DI.Modules;
using Plutus.Core.Helpers;
using Plutus.Core.Interfaces;
using Xunit;

namespace Plutus.Tests.DatabaseTests
{
    public class RedisTests : TestBase
    {
        public RedisTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<DatabaseModule>();

            SetContainer(builder);

            var handler = Container.Resolve<IDatabaseHandler>();
            handler.FlushDatabase();
        }

        [Fact]
        public async void CanAddOrder()
        {
            var handler = Container.Resolve<IDatabaseHandler>();

            var order = new Order
            {
                Base = "BTC",
                Test = true,
                Amount = 15.2m,
                Price = 9.8m,
                Symbol = "XRP",
                LossStop = 10m,
                ProfitStop = 12m
            };

            var key = await handler.AddToDatabase(order);

            var returnOrder = await handler.GetFromDatabase(key);

            Assert.True(order.Test.Equals(returnOrder.Test) &&
                        order.Base.Equals(returnOrder.Base) &&
                        order.Amount.Equals(returnOrder.Amount) &&
                        order.Symbol.Equals(returnOrder.Symbol) &&
                        order.Price.Equals(returnOrder.Price) &&
                        order.LossStop.Equals(returnOrder.LossStop) &&
                        order.ProfitStop.Equals(returnOrder.ProfitStop));
        }

        [Fact]
        public async void CanGetIndexKeys()
        {
            var handler = Container.Resolve<IDatabaseHandler>();

            for (int i = 0; i < 5; i++)
            {
                var order = new Order
                {
                    Base = "BTC",
                    Test = true,
                    Amount = 15.2m,
                    Price = 9.8m,
                    Symbol = "XRP",
                    LossStop = 10m,
                    ProfitStop = 12m
                };

                await handler.AddToDatabase(order);
            }

            var keys = await handler.GetAllKeys();

            Assert.True(keys.Count() == 5);
        }
    }
}