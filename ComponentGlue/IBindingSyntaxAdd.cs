﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue
{
    public interface IBindingSyntaxAdd
    {
        IBindingSyntaxAs<IBindingSyntaxAddOrWith> Add(Type type);
    }
}
