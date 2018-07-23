using System.Collections.Generic;
using System.Threading.Tasks;
using Plutus.Core.Helpers;

namespace Plutus.Core.Interfaces
{
    public interface IDatabaseHandler
    {
        Task<string> AddToDatabase(Order order);

        Task<Order> GetFromDatabase(string key);

        Task<IEnumerable<string>> GetAllKeys();
        
        void FlushDatabase();
    }
}