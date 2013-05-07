using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	public class Foo1 : IFoo
	{
		public IBar Bar
		{
			get;
			private set;
		}
		
		[Inject]
		public Foo1(IBar bar)
		{
			this.Bar = bar;
		}
	}
}
