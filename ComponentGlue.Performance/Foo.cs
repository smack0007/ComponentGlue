using System;

namespace ComponentGlue.Performance
{
    public class Foo : IFoo
    {
        public IBar Bar
        {
            get;
            private set;
        }

        public IBaz Baz
        {
            get;
            private set;
        }

        public Foo(IBar bar, IBaz baz)
        {
            if (bar == null)
                throw new ArgumentException("bar");

            if (baz == null)
                throw new ArgumentNullException("baz");

            this.Bar = bar;
            this.Baz = baz;
        }
    }
}
