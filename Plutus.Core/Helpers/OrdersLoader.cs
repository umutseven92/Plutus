using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Plutus.Core.Interfaces;

namespace Plutus.Core.Helpers
{
    public class OrdersLoader : IOrdersLoader
    {
        public List<OrderConfiguration> Orders { get; set; }

        public OrdersLoader(string path)
        {
            var ordersJson = File.ReadAllText(path);
            var token = JObject.Parse(ordersJson);
            
            var orders = token["Orders"];

            Orders = new List<OrderConfiguration>();
            
            foreach (var order in orders)
            {
                var orderBase = order["Base"].Value<string>();
                var symbol = order["Symbol"].Value<string>();
                var plSymbol = order["PLSymbol"].Value<string>();
                var profitStop = order["ProfitStop"].Value<decimal>();
                var lossStop = order["LossStop"].Value<decimal>();
                var buyAmount = order["BuyAmount"].Value<decimal>();
                
                Orders.Add(new OrderConfiguration()
                {
                    Base = orderBase,
                    Symbol = symbol,
                    LossStop = lossStop,
                    ProfitStop = profitStop,
                    BuyAmount = buyAmount
                });
            }
        }
    }
}