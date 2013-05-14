using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class StubObjectBarConstructor : IStubObject
    {
        public string ID
        {
            get;
            set;
        }

        public StubObjectBarConstructor(string id)
        {
            this.ID = id;
        }
    }
}
