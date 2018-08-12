using Autofac;
using System;

namespace Plutus.Tests.Unit
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