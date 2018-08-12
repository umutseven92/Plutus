using System.Collections.Generic;
using System.Threading.Tasks;
using Plutus.Core.Enums;

namespace Plutus.Core.Interfaces
{
    public interface IBinanceHandler
    {
        Task<bool> Ping();

        Task<decimal> GetPrice(string orderBase, string orderSymbol);

        Task<decimal?> GetBalance(string symbol);

        Task<IEnumerable<decimal>> GetClosingPrices(string orderBase, string orderSymbol, Period period);

        Task Buy(string orderBase, string orderSymbol, decimal amount);

        Task Sell(string orderBase, string orderSymbol, decimal amount);
        
        Task BuyTest(string orderBase, string orderSymbol, decimal amount);

        Task SellTest(string orderBase, string orderSymbol, decimal amount);
    }
}