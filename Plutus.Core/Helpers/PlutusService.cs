using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plutus.Core.Enums;
using Plutus.Core.Interfaces;

namespace Plutus.Core.Helpers
{
    public class PlutusService : IPlutusService
    {
        public IList<OrderConfiguration> Orders { get; }

        private readonly IBinanceHandler _binanceHandler;
        private readonly ITAHandler _taHandler;
        private readonly ILogger _logger;
        private readonly IDatabaseHandler _databaseHandler;

        public PlutusService(
            IBinanceHandler binanceHandler, 
            IOrdersLoader ordersLoader,
            ILogger logger,
            ITAHandler taHandler, 
            IDatabaseHandler databaseHandler)
        {
            _binanceHandler = binanceHandler;
            Orders = ordersLoader.Orders;
            _logger = logger;
            _taHandler = taHandler;
            _databaseHandler = databaseHandler;
        }

        public async Task<decimal> GetPrice(string orderBase, string orderSymbol)
        {
            var price = await _binanceHandler.GetPrice(orderBase, orderSymbol);
            _logger.LogInformation($"Price for {orderBase}{orderSymbol} is {price}");

            return price;
        }

        public async Task Sell(string key, string orderSymbol, string orderBase, decimal sellAmount, decimal newPrice)
        {
            await _binanceHandler.Sell(orderBase, orderSymbol, sellAmount);
            
            _logger.LogInformation(
                $"Sold {sellAmount} {orderSymbol} for {newPrice * sellAmount} {orderBase} ({newPrice} a piece) | SELLID: {key}");
        }
        
        public async Task SellTest(string key, string orderSymbol, string orderBase, decimal sellAmount, decimal newPrice)
        {
            await _binanceHandler.SellTest(orderBase, orderSymbol, sellAmount);
            
            _logger.LogInformation(
                $"Test sold {sellAmount} {orderSymbol} for {newPrice * sellAmount} {orderBase} ({newPrice} a piece) | SELLID: {key}");
        }
        
        public async Task Buy(string orderSymbol, string orderBase, decimal buyAmount, decimal price)
        {
            await _binanceHandler.Buy(orderBase, orderSymbol, buyAmount);
            
            var order = new Order
            {
                Amount = buyAmount,
                Base = orderBase,
                Price = price,
                Symbol = orderSymbol,
                Test = false
            };

            var key = await _databaseHandler.AddToDatabase(order);
            _logger.LogInformation(
                $"Bought {buyAmount} {orderSymbol} for {price * buyAmount} {orderBase} ({price} a piece) | BUYID: {key}");
        }

        public async Task BuyTest(string orderSymbol, string orderBase, decimal buyAmount, decimal price)
        {
            await _binanceHandler.BuyTest(orderBase, orderSymbol, buyAmount);

            var order = new Order
            {
                Amount = buyAmount,
                Base = orderBase,
                Price = price,
                Symbol = orderSymbol,
                Test = true
            };

            var key = await _databaseHandler.AddToDatabase(order);
            _logger.LogInformation(
                $"Test bought {buyAmount} {orderSymbol} for {price * buyAmount} {orderBase} ({price} a piece) | BUYID: {key}");
        }

        public async Task<bool> CheckBalance(decimal cost, string balanceBase)
        {
            var balance = await GetBalance(balanceBase);

            if (balance >= cost)
            {
                _logger.LogInformation($"Balance is enough to cover {cost}.");
                return true;
            }
            else
            {
                _logger.LogInformation($"Balance is NOT enough to cover {cost}.");
                return false;
            }
        }

        private async Task<decimal> GetBalance(string balanceBase)
        {
            var balance = await _binanceHandler.GetBalance(balanceBase);

            var balanceValue = balance ?? 0m;

            _logger.LogInformation($"Balance for {balanceBase} is {balanceValue}.");

            return balanceValue;
        }

        public async Task<PricePrediction> GetPricePrediction(string orderBase, string orderSymbol)
        {
            var candleSticks = await _binanceHandler.GetCandlesticks(orderBase, orderSymbol);

            var prediction = _taHandler.GetPricePrediction(candleSticks);
            _logger.LogInformation($"Prediction for {orderSymbol} is {prediction.ToString()}.");

            return prediction;
        }

        public async Task<List<Tuple<string, Order>>> GetAllOpenOrders()
        {
            var keys = (await _databaseHandler.GetAllKeys()).ToList();

            var orders = new List<Tuple<string,Order>>(keys.Count());
            foreach (var key in keys)
            {
                var order = await _databaseHandler.GetFromDatabase(key);
                orders.Add(new Tuple<string, Order>(key, order));
            }

            return orders;
        }

        public async Task<bool> IsOnline()
        {
            var online = await _binanceHandler.Ping();

            return online;
        }
    }
}