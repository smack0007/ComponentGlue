using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class StubObjectFooConstructor : IStubObject
    {
        public string ID
        {
            get;
            set;
        }

        public StubObjectFooConstructor(string id)
        {
            this.ID = id;
        }
    }
}
