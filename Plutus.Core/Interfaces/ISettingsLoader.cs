using System.Collections.Generic;
using Plutus.Core.Helpers;

namespace Plutus.Core.Interfaces
{
    public interface ISettingsLoader
    {
        int BuyInterval { get; }

        int SellInterval { get; }

        bool Test { get; }

        string RedisUrl { get; }
        
        List<ExhangeConfig> ExhangeConfigurations { get; }
    }
}