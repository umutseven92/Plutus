using System.Threading.Tasks;
using FluentScheduler;
using Plutus.Core.Interfaces;

namespace Plutus.Core.Jobs
{
    public class SellJob : IJob
    {
        private readonly IPlutusService _plutusService;
        private readonly bool _test;

        public SellJob(IPlutusService plutusService, bool test)
        {
            _plutusService = plutusService;
            _test = test;
        }

        public async void Execute()
        {
            return;
            // Get all open orders
            var orders = await _plutusService.GetAllOpenOrders();

            Parallel.ForEach(orders, async (orderTuple) =>
            {
                var order = orderTuple.Item2;
                var key = orderTuple.Item1;
                
                // Get the current price of the order 
                var newPrice = await _plutusService.GetPrice(order.Base, order.Symbol);

                // If current price is bigger than price + ProfitStop, OR if current price is lower than price - LossStop, sell
                if (newPrice > order.Price + order.ProfitStop ||
                    newPrice < order.Price - order.LossStop)
                {
                    if (_test)
                    {
                        await _plutusService.SellTest(key, order.Symbol, order.Base, order.Amount, newPrice);
                    }
                    else
                    {
                        await _plutusService.Sell(key, order.Symbol, order.Base, order.Amount, newPrice);
                    }
                }
            });
        }
    }
}