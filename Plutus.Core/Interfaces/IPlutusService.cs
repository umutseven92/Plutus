using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plutus.Core.Enums;
using Plutus.Core.Helpers;

namespace Plutus.Core.Interfaces
{
    public interface IPlutusService
    {
        IList<OrderConfiguration> Orders { get; }

        Task<decimal> GetPrice(string orderBase, string orderSymbol);

        Task Buy(string orderSymbol, string orderBase, decimal buyAmount, decimal price);
        
        Task BuyTest(string orderSymbol, string orderBase, decimal buyAmount, decimal price);

        Task Sell(string key, string orderSymbol, string orderBase, decimal sellAmount, decimal newPrice);

        Task SellTest(string key, string orderSymbol, string orderBase, decimal sellAmount, decimal newPrice);

        Task<bool> CheckBalance(decimal cost, string balanceBase);

        Task<PricePrediction> GetPricePrediction(string orderBase, string orderSymbol, Period period);

        Task<bool> IsOnline();
        
        Task<List<Tuple<string, Order>>> GetAllOpenOrders();

    }
}