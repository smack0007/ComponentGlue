using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    public interface IBindingSyntaxAddOrAs : IBindingSyntaxAdd, IBindingSyntaxAs<IBindingSyntaxAddOrWith>
    {
    }
}
