using System;
using ComponentGlue;

namespace ComponentGlue.Demo
{
	public class Foo : IFoo
	{
		public IBar Bar
		{
			get;
			private set;
		}

        public string Name
        {
            get;
            private set;
        }

		public Foo(IBar bar, string name)
		{
			this.Bar = bar;
            this.Name = name;
		}
	}
}
