using System;

namespace ComponentGlue.Performance
{
    public class Bar : IBar
    {
        public IBaz Baz
        {
            get;
            private set;
        }

        public Bar(IBaz baz)
        {
            if (baz == null)
                throw new ArgumentNullException("baz");

            this.Baz = baz;
        }
    }
}
