using System;
using Autofac;
using FluentScheduler;
using Microsoft.Extensions.Logging;
using Plutus.Core.DI;
using Plutus.Core.Helpers;
using Plutus.Core.Interfaces;

namespace Plutus.Server
{
    internal static class Program
    {
        private static ILogger _logger;

        private static void Main(string[] args)
        {
            var container = AutofacBootstrapper.Init();
            var plutusService = container.Resolve<IPlutusService>();
            var settings = container.Resolve<ISettingsLoader>();
            _logger = container.Resolve<ILogger>();

            JobManager.Initialize(new PlutusRegistry(plutusService, settings));

            JobManager.JobException += HandleJobException;

            Console.ReadKey();
            JobManager.Stop();
        }

        private static void HandleJobException(JobExceptionInfo info)
        {
            _logger.LogError($"An error just happened with the scheduled job {info.Name}: {info.Exception}");
            
            JobManager.Stop();
        }
    }
}