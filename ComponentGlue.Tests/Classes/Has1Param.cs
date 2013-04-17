using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class Has1Param
    {
        public string Name
        {
            get;
            private set;
        }

        public Has1Param(string name)
        {
            this.Name = name;
        }
    }
}
