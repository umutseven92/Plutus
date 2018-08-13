using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance;
using Plutus.Core.Enums;
using Plutus.Core.Interfaces;

namespace Plutus.Core.ExchangeHandlers
{
    public class BinanceHandler : IBinanceHandler
    {
        private readonly BinanceApiUser _user;
        private readonly BinanceApi _binanceClient;

        public BinanceHandler(BinanceApiUser user, BinanceApi binanceClient)
        {
            _user = user;
            _binanceClient = binanceClient;
        }

        public async Task<bool> Ping()
        {
            var online = await _binanceClient.PingAsync();

            return online;
        }

        public async Task<decimal?> GetBalance(string symbol)
        {
            var accountInfo = await _binanceClient.GetAccountInfoAsync(_user);

            var balance = accountInfo.Balances.FirstOrDefault(b =>
                b.Asset.Equals(symbol, StringComparison.InvariantCultureIgnoreCase));

            return balance?.Free;
        }

        public async Task<decimal> GetPrice(string orderBase, string orderSymbol)
        {
            var symbol = $"{orderSymbol}/{orderBase}";

            var price = await _binanceClient.GetPriceAsync(symbol);

            return price.Value;
        }

        public async Task<IEnumerable<decimal>> GetClosingPrices(string orderBase, string orderSymbol, Period period, int take)
        {
            var symbol = $"{orderSymbol}/{orderBase}";

            var candleStickInterval = ConvertPeriodToCandlestickInterval(period);
            
            var candlesticks = await _binanceClient.GetCandlesticksAsync(symbol, candleStickInterval, take);

            var closingPrices = ConvertCandlestickToClosingPrices(candlesticks);
            
            return closingPrices;
        }
        
        public async Task BuyTest(string orderBase, string orderSymbol, decimal amount)
        {
            await OrderBaseTest(OrderSide.Buy, orderBase, orderSymbol, amount);
        }

        public async Task SellTest(string orderBase, string orderSymbol, decimal amount)
        {
            await OrderBaseTest(OrderSide.Sell, orderBase, orderSymbol, amount);
        }
        
        private async Task OrderBaseTest(OrderSide order, string orderBase, string orderSymbol, decimal amount)
        {
            var clientOrder = new MarketOrder(_user)
            {
                Symbol = $"{orderSymbol}/{orderBase}",
                Side = order,
                Quantity = amount,
            };
 
            // Validate client order.
            clientOrder.Validate();

            // Send the TEST order.
            await _binanceClient.TestPlaceAsync(clientOrder);
        }

        public async Task Buy(string orderBase, string orderSymbol, decimal amount)
        {
            await OrderBase(OrderSide.Buy, orderBase, orderSymbol, amount);
        }

        public async Task Sell(string orderBase, string orderSymbol, decimal amount)
        {
            await OrderBase(OrderSide.Sell, orderBase, orderSymbol, amount);
        }
        
        private async Task OrderBase(OrderSide order, string orderBase, string orderSymbol, decimal amount)
        {
            throw new NotImplementedException();
            var clientOrder = new MarketOrder(_user)
            {
                Symbol = $"{orderBase}{orderSymbol}",
                Side = order,
                Quantity = amount
            };

            // Validate client order.
            clientOrder.Validate();

            // Send the order.
            await _binanceClient.PlaceAsync(clientOrder);
        }

        private CandlestickInterval ConvertPeriodToCandlestickInterval(Period period)
        {
            switch (period)
            {
                case Period.Hourly:
                    return CandlestickInterval.Hour;
                case Period.Daily:
                    return CandlestickInterval.Day;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<decimal> ConvertCandlestickToClosingPrices(IEnumerable<Candlestick> candlesticks)
        {
            var closingPrices = candlesticks.Select(c => c.Close);

            return closingPrices;
        }
    }
}