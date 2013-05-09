using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    internal class FactoryMethodComponentBindingStrategy<T> : IComponentBindingStrategy
    {
        Func<IComponentResolver, T> factoryMethod;

        public FactoryMethodComponentBindingStrategy(Func<IComponentResolver, T> factoryMethod)
        {
            if (factoryMethod == null)
                throw new ArgumentNullException("factoryMethod");

            this.factoryMethod = factoryMethod;
        }

        public object Resolve(IComponentContainer container)
        {
            return this.factoryMethod(container);
        }
    }
}
