﻿using Autofac;
using Plutus.Core.DI.Modules;

namespace Plutus.Tests.LoaderTests
{
    public class LoaderTestBase:TestBase
    {
        protected LoaderTestBase()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<ConfigurationModule>();

            base.SetContainer(builder);
        }
    }
}