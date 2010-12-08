using System;
using ComponentGlue.Framework;

namespace ComponentGlue.Tests.Classes
{
	public class PropertyInject
	{
		[InjectComponent]
		public IFoo Foo
		{
			get;
			private set;
		}

		[InjectComponent]
		public IBar Bar
		{
			get;
			private set;
		}
	}
}
