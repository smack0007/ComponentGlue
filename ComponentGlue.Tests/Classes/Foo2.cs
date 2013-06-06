using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentGlue.Tests.Classes
{
    public class Foo2 : IFoo
    {
        public IBar Bar
        {
            get;
            private set;
        }

        [Resolve]
        public Foo2(IBar bar)
        {
            this.Bar = bar;
        }
    }
}
