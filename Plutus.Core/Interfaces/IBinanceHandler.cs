using System.Collections.Generic;
using System.Threading.Tasks;
using Trady.Core;

namespace Plutus.Core.Interfaces
{
    public interface IBinanceHandler
    {
        Task<bool> Ping();

        Task<decimal> GetPrice(string orderBase, string orderSymbol);

        Task<decimal?> GetBalance(string symbol);

        Task<IEnumerable<Candle>> GetCandlesticks(string orderBase, string orderSymbol);

        Task Buy(string orderBase, string orderSymbol, decimal amount);

        Task Sell(string orderBase, string orderSymbol, decimal amount);
        
        Task BuyTest(string orderBase, string orderSymbol, decimal amount);

        Task SellTest(string orderBase, string orderSymbol, decimal amount);
    }
}