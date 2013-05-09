using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    internal interface IComponentBindingStrategy
    {
        object Resolve(IComponentContainer container);
    }
}
