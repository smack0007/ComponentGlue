using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests
{
    public class TestAutoBindingStrategy : IAutoBindStrategy
    {
        Action<ComponentContainer, Type, IList<Type>> bind;

        public TestAutoBindingStrategy(Action<ComponentContainer, Type, IList<Type>> bind)
        {
            this.bind = bind;
        }

        public void Bind(ComponentContainer container, Type interfaceType, IList<Type> componentTypes)
        {
            this.bind(container, interfaceType, componentTypes);
        }
    }
}
