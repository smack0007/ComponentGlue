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
		
		[Inject]
		public Foo(IBar bar)
		{
			this.Bar = bar;
		}
	}
}
