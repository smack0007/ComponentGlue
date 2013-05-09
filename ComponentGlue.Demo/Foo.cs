using System;
using ComponentGlue;

namespace ComponentGlue.Demo
{
	public class Foo
	{
		public IBar Bar
		{
			get;
			private set;
		}
                
		public Foo(IBar bar)
		{
			this.Bar = bar;
		}
	}
}
