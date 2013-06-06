using System;

namespace ComponentGlue.Tests
{
    class TestBindingStrategy<T> : IComponentBindingStrategy, IDisposable
    {
        Func<T> factory;
        public bool ResolveWasCalled = false;
        public bool DisposeWasCalled = false;

        public TestBindingStrategy(Func<T> factory)
        {
            this.factory = factory;
        }

        public object Resolve(ComponentContainer container)
        {
            ResolveWasCalled = true;
            return this.factory();
        }

        public void Dispose()
        {
            DisposeWasCalled = true;
        }
    }
}
