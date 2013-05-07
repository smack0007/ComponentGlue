using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class StubObjectBar : IStubObject
    {
        public string ID
        {
            get;
            set;
        }

        public StubObjectBar(string id)
        {
            this.ID = id;
        }
    }
}
