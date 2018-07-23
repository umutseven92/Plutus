using Autofac;
using Plutus.Core.Interfaces;
using Xunit;

namespace Plutus.Tests.LoaderTests
{
    public class SettingsLoaderTests : LoaderTestBase
    {
        private const int BuyInterval = 2500;
        private const int SellInterval = 1000;
        private const string RedisUrl = "localhost:6379";
        private const bool Test = true;

        [Fact]
        public void CanLoadSettings()
        {
            var loader = Container.Resolve<ISettingsLoader>();

            Assert.True(loader.BuyInterval.Equals(BuyInterval));
            Assert.True(loader.SellInterval.Equals(SellInterval));
            Assert.Equal(loader.RedisUrl, RedisUrl);
            Assert.True(loader.Test.Equals(Test));

            Assert.True(loader.ExhangeConfigurations.Count == 1);
            Assert.Contains(loader.ExhangeConfigurations, b => b.ExhangeName.Equals("Binance"));
        }
    }
}