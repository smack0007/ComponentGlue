using System;

namespace ComponentGlue
{
    public interface IComponentBindingStrategy
    {
        object Resolve(ComponentContainer container);
    }
}
