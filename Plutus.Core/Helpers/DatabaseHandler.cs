using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Plutus.Core.Interfaces;
using StackExchange.Redis;

namespace Plutus.Core.Helpers
{
    public class DatabaseHandler : IDatabaseHandler
    {
        private const string Index = "Index";
        private readonly IConnectionMultiplexer _iConnectionMultiplexer;
        private readonly ISettingsLoader _iSettingsLoader;

        public DatabaseHandler(
            IConnectionMultiplexer iConnectionMultiplexer, 
            ISettingsLoader iSettingsLoader)
        {
            _iConnectionMultiplexer = iConnectionMultiplexer;
            _iSettingsLoader = iSettingsLoader;
        }

        private async Task AddKeyToIndex(string key)
        {
            var db = _iConnectionMultiplexer.GetDatabase();
            await db.SetAddAsync(Index, key);
        }

        public void FlushDatabase()
        {
            _iConnectionMultiplexer.GetServer(_iSettingsLoader.RedisUrl).FlushDatabase();
        }

        private async Task RemoveKeyFromIndex(string key)
        {
            var db = _iConnectionMultiplexer.GetDatabase();

            await db.SetRemoveAsync(Index, key);
        }

        public async Task<string> AddToDatabase(Order order)
        {
            var db = _iConnectionMultiplexer.GetDatabase();
            var timestamp = DateTime.Now.ToString("O");

            var key = Guid.NewGuid().ToString();
            var values = new[]
            {
                new HashEntry("Test", order.Test),
                new HashEntry("Base", order.Base),
                new HashEntry("Symbol", order.Symbol),
                new HashEntry("Price", order.Price.ToString(CultureInfo.InvariantCulture)),
                new HashEntry("Amount", order.Amount.ToString(CultureInfo.InvariantCulture)),
                new HashEntry("Timestamp", $"{timestamp}"),
                new HashEntry("LossStop", order.LossStop.ToString(CultureInfo.InvariantCulture)),
                new HashEntry("ProfitStop", order.ProfitStop.ToString(CultureInfo.InvariantCulture)),
            };

            await db.HashSetAsync(key, values);

            await AddKeyToIndex(key);

            return key;
        }

        public async Task<Order> GetFromDatabase(string key)
        {
            var db = _iConnectionMultiplexer.GetDatabase();

            var hash = await db.HashGetAllAsync(key);

            var price = Convert.ToDecimal(hash.First(h => h.Name.Equals("Price")).Value, CultureInfo.InvariantCulture);
            var amount = Convert.ToDecimal(hash.First(h => h.Name.Equals("Amount")).Value, CultureInfo.InvariantCulture);
            var lossStop = Convert.ToDecimal(hash.First(h => h.Name.Equals("LossStop")).Value, CultureInfo.InvariantCulture);
            var profitStop = Convert.ToDecimal(hash.First(h => h.Name.Equals("ProfitStop")).Value, CultureInfo.InvariantCulture);

            return new Order()
            {
                Base = hash.First(h => h.Name.Equals("Base")).Value,
                Symbol = hash.First(h => h.Name.Equals("Symbol")).Value,
                Test = hash.First(h => h.Name.Equals("Test")).Value == 1,
                Price = price,
                Amount = amount,
                LossStop = lossStop,
                ProfitStop = profitStop
            };
        }

        public async Task<IEnumerable<string>> GetAllKeys()
        {
            var db = _iConnectionMultiplexer.GetDatabase();

            var members = await db.SetMembersAsync(Index);

            return members.Select(r => r.ToString());
        }

        public async Task RemoveFromDatabase(string key)
        {
            await RemoveKeyFromIndex(key);
        }
    }
}