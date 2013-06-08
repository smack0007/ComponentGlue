using System;
using System.Collections.Generic;

namespace ComponentGlue
{
    public interface IAutoBindStrategy
    {
        void Bind(ComponentContainer container, Type interfaceType, IList<Type> componentTypes);
    }
}
