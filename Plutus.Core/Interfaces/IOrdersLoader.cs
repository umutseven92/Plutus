using System.Collections.Generic;
using Plutus.Core.Helpers;

namespace Plutus.Core.Interfaces
{
    public interface IOrdersLoader
    {
         List<OrderConfiguration> Orders { get; set; }
    }
}