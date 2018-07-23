using System.Collections.Generic;
using Plutus.Core.Enums;
using Trady.Core;

namespace Plutus.Core.Interfaces
{
    public interface ITAHandler
    {
        decimal? CalculateRSI(IEnumerable<Candle> candles);

        PricePrediction GetPricePrediction(IEnumerable<Candle> candles);
    }
}