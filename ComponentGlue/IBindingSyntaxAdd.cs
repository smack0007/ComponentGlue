using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    public interface IBindingSyntaxAdd
    {
        IBindingSyntaxAsWithMultiple Add(Type type);
    }
}
