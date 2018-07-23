using System.Collections.Generic;
using System.Linq;
using Plutus.Core.Enums;
using Plutus.Core.Interfaces;
using Trady.Analysis;
using Trady.Core;

namespace Plutus.Core.Helpers
{
    public class TAHandler : ITAHandler
    {
        private const int PeriodCount = 14;
        private const int RSIUpper = 70;
        private const int RSILower = 30;
        private const int RSIMiddle = 50;
        
        public decimal? CalculateRSI(IEnumerable<Candle> candles)
        {
            var rsiAll = candles.Rsi(PeriodCount);

            var rsi = rsiAll.Last();

            return rsi.Tick;
        }

        public PricePrediction GetPricePrediction(IEnumerable<Candle> candles)
        {
            var rsi = CalculateRSI(candles);

            if (rsi.HasValue && GetRSIPrediction(rsi.Value) == RSIInterpretation.Bullish)
            {
                return PricePrediction.Bullish;
            }

            return PricePrediction.Bearish;
        }

        private static RSIInterpretation GetRSIPrediction(decimal rsi)
        {
            if (rsi >= 0 && rsi < RSILower)
            {
                return RSIInterpretation.Oversold;
            }

            if(rsi >= RSILower && rsi < RSIMiddle)
            {
                return RSIInterpretation.Bearish;
            }

            if(rsi >= RSIMiddle && rsi < RSIUpper)
            {
                return RSIInterpretation.Bullish;
            }

            if(rsi >= RSIUpper && rsi <= 100)
            {
                return RSIInterpretation.Overbought;
            }

            return RSIInterpretation.Unknown;
        }

    }
}