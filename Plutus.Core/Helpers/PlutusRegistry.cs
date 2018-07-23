using FluentScheduler;
using Plutus.Core.Interfaces;
using Plutus.Core.Jobs;

namespace Plutus.Core.Helpers
{
    public class PlutusRegistry : Registry
    {
        public PlutusRegistry(IPlutusService plutusService, ISettingsLoader settings)
        {
            if (settings.BuyInterval == 0)
            {
                Schedule((() => new BuyJob(plutusService, settings.Test))).ToRunNow();
            }
            else
            {
                Schedule((() => new BuyJob(plutusService, settings.Test))).ToRunNow().AndEvery(settings.BuyInterval)
                    .Milliseconds();
            }

            if (settings.SellInterval == 0)
            {
                Schedule((() => new SellJob(plutusService, settings.Test))).ToRunNow();
            }
            else
            {
                Schedule((() => new SellJob(plutusService, settings.Test))).ToRunNow().AndEvery(settings.SellInterval)
                    .Milliseconds();
            }
        }
    }
}