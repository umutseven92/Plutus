using System.Threading.Tasks;
using FluentScheduler;
using Plutus.Core.Enums;
using Plutus.Core.Interfaces;

namespace Plutus.Core.Jobs
{
    public class BuyJob : IJob
    {
        private readonly IPlutusService _plutusService;
        private readonly bool _test;

        public BuyJob(IPlutusService plutusService, bool test)
        {
            _plutusService = plutusService;
            _test = test;
        }

        public void Execute()
        {
            Parallel.ForEach(_plutusService.Orders, async (order) =>
            {
                // Check price of symbol
                var price = await _plutusService.GetPrice(order.Base, order.Symbol);

                // Determine if its going up or down
                var prediction = await _plutusService.GetPricePrediction(order.Base, order.Symbol);

                if (prediction == PricePrediction.Bullish)
                {
                    // If its going up,
                    var buyAmount = order.BuyAmount;

                    // Calculate cost
                    var cost = price * buyAmount;

                    // Check if balance is sufficent
                    var isBalanceSufficent = await _plutusService.CheckBalance(cost, order.Base);

                    if (isBalanceSufficent)
                    {
                        // If balance is sufficient, buy
                        if (_test)
                        {
                            await _plutusService.BuyTest(order.Symbol, order.Base, buyAmount, price);
                        }
                        else
                        {
                            await _plutusService.Buy(order.Symbol, order.Base, buyAmount, price);
                        }
                    }
                }
            });
        }
    }
}