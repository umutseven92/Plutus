using System.Collections.Generic;
using System.Linq;
using Plutus.Core.Enums;
using Plutus.Core.Interfaces;

namespace Plutus.Core.Helpers
{
    public class TAHandler : ITAHandler
    {
        public decimal CalculateSMA(IEnumerable<decimal> closingPrices)
        {
            var closingPricesList = closingPrices.ToList();
            var sma = closingPricesList.Sum() / closingPricesList.Count();

            return sma;
        }

        public PricePrediction GetPricePrediction(IEnumerable<decimal> closingPrices, decimal currentPrice)
        {
            var sma = CalculateSMA(closingPrices);

            if (sma > currentPrice)
            {
                return PricePrediction.Bullish;
            }
            else
            {
                return PricePrediction.Bearish;
            }
        }
    }
}