using System;
using ComponentGlue;

namespace ComponentGlue.Tests.Classes
{
	public class PropertyInject
	{
		[Resolve]
		public IFoo Foo
		{
			get;
			private set;
		}

		[Resolve]
		public IBar Bar
		{
			get;
			private set;
		}
	}
}
