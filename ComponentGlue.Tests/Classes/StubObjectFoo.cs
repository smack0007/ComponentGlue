using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class StubObjectFoo : IStubObject
    {
        public string ID
        {
            get;
            set;
        }

        public StubObjectFoo(string id)
        {
            this.ID = id;
        }
    }
}
