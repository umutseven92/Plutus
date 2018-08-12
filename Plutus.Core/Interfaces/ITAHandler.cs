using System.Collections.Generic;
using Plutus.Core.Enums;

namespace Plutus.Core.Interfaces
{
    public interface ITAHandler
    {
        decimal CalculateSMA(IEnumerable<decimal> closingPrices);

        PricePrediction GetPricePrediction(IEnumerable<decimal> closingPrices, decimal currentPrice);
    }
}