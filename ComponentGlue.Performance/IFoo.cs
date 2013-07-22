using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Performance
{
    public interface IFoo
    {
        IBar Bar { get; }

        IBaz Baz { get; }
    }
}
