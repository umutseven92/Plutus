using System.Collections.Generic;
using Autofac;
using Plutus.Core.DI.Modules;
using Plutus.Core.Interfaces;
using Xunit;

namespace Plutus.Tests.Unit.TATests
{
    public class CalculationTests: TestBase
    {
        public CalculationTests()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<TAModule>();

            base.SetContainer(builder);
        }
        
        [Fact]
        public void CanCalculateSMA()
        {
            var taHandler = Container.Resolve<ITAHandler>();

            var closingPrices = new List<decimal>()
            {
                15, 6, 10, 15, 9, 7, 11, 12, 14, 11
            };

            var sma = taHandler.CalculateSMA(closingPrices);
            
            Assert.Equal(sma, 11);
        }
    }
}