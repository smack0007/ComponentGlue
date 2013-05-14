using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    internal class ConstantComponentBindingStrategy : IComponentBindingStrategy
    {
        object value;

        public ConstantComponentBindingStrategy(object value)
        {
            this.value = value;
        }

        public object Resolve(ComponentContainer container)
        {
            return this.value;
        }
    }
}
