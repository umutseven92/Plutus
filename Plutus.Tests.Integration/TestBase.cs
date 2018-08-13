using System;
using Autofac;

namespace Plutus.Tests.Integration
{
    public class TestBase: IDisposable
    {
        protected IContainer Container;

        protected void SetContainer(ContainerBuilder builder)
        {
            Container = builder.Build();
        }

        public void Dispose()
        {
            Container?.Dispose();
        }
    }
}