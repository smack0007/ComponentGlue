using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class NeedsArrayOfStubObjects
    {
        public IStubObject[] Stubs
        {
            get;
            private set;
        }

        public NeedsArrayOfStubObjects(IStubObject[] stubs)
        {
            if (stubs == null)
                throw new ArgumentNullException("stubs");

            this.Stubs = stubs;
        }
    }
}
