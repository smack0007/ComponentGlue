using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class Has1PropertyWithNoSetter
    {
        string id;

        public string ID
        {
            get { return this.id; }
        }
    }
}
